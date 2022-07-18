using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NSE.Bff.Compras.Configuration;
using NSE.Bff.Compras.Models;

namespace NSE.Bff.Compras.Services
{
    public interface IClienteService
    {
        Task<EnderecoDTO> ObterEndereco();
    }

    public class ClienteService : Service, IClienteService
    {
        private readonly HttpClient _httpClient;
        private readonly BaseUrlsConfiguration _baseUrlsConfiguration;

        public ClienteService(HttpClient httpClient, IOptions<BaseUrlsConfiguration> baseUrlsConfiguration)
        {
            _httpClient = httpClient;
            _baseUrlsConfiguration = baseUrlsConfiguration.Value;

            _httpClient.BaseAddress = new Uri(_baseUrlsConfiguration.ApiClienteUrl);
        }

        public async Task<EnderecoDTO> ObterEndereco()
        {
            var response = await _httpClient.GetAsync("/cliente/endereco/");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<EnderecoDTO>(response);
        }
    }
}
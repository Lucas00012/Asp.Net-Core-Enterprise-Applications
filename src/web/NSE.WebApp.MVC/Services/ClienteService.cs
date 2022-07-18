using Microsoft.Extensions.Options;
using NSE.Core.Communication;
using NSE.WebApp.MVC.Configuration;
using NSE.WebApp.MVC.Models;
using System.Net;

namespace NSE.WebApp.MVC.Services
{
    public interface IClienteService
    {
        Task<EnderecoViewModel> ObterEndereco();
        Task<ResponseResult> AdicionarEndereco(EnderecoViewModel endereco);
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

        public async Task<EnderecoViewModel> ObterEndereco()
        {
            var response = await _httpClient.GetAsync("/cliente/endereco/");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<EnderecoViewModel>(response);
        }

        public async Task<ResponseResult> AdicionarEndereco(EnderecoViewModel endereco)
        {
            var enderecoContent = ObterConteudo(endereco);

            var response = await _httpClient.PostAsync("/cliente/endereco/", enderecoContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

            return RetornoOk();
        }
    }
}

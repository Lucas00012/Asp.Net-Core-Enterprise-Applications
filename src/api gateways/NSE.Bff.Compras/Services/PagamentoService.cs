using System;
using System.Net.Http;
using Microsoft.Extensions.Options;
using NSE.Bff.Compras.Configuration;

namespace NSE.Bff.Compras.Services
{
    public interface IPagamentoService
    {

    }

    public class PagamentoService : Service, IPagamentoService
    {
        private readonly HttpClient _httpClient;
        private readonly BaseUrlsConfiguration _baseUrlsConfiguration;

        public PagamentoService(HttpClient httpClient, IOptions<BaseUrlsConfiguration> baseUrlsConfiguration)
        {
            _baseUrlsConfiguration = baseUrlsConfiguration.Value;
            _httpClient = httpClient;

            //_httpClient.BaseAddress = new Uri(_baseUrlsConfiguration.ApiPagamentoUrl);
        }
    }
}
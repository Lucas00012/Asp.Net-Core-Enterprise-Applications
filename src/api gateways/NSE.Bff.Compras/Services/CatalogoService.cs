using System;
using System.Net.Http;
using Microsoft.Extensions.Options;
using NSE.Bff.Compras.Configuration;
using NSE.Bff.Compras.Models;

namespace NSE.Bff.Compras.Services
{
    public interface ICatalogoService
    {
        Task<ItemProdutoDTO> ObterPorId(Guid id);
        Task<IEnumerable<ItemProdutoDTO>> ObterItens(IEnumerable<Guid> ids);
    }

    public class CatalogoService : Service, ICatalogoService
    {
        private readonly HttpClient _httpClient;
        private readonly BaseUrlsConfiguration _baseUrlsConfiguration;

        public CatalogoService(HttpClient httpClient, IOptions<BaseUrlsConfiguration> baseUrlsConfiguration)
        {
            _baseUrlsConfiguration = baseUrlsConfiguration.Value;
            _httpClient = httpClient;

            _httpClient.BaseAddress = new Uri(_baseUrlsConfiguration.ApiCatalogoUrl);
        }

        public async Task<ItemProdutoDTO> ObterPorId(Guid id)
        {
            var response = await _httpClient.GetAsync($"/catalogo/produtos/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<ItemProdutoDTO>(response);
        }

        public async Task<IEnumerable<ItemProdutoDTO>> ObterItens(IEnumerable<Guid> ids)
        {
            var idsRequest = string.Join(",", ids);

            var response = await _httpClient.GetAsync($"/catalogo/produtos/lista/{idsRequest}/");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<IEnumerable<ItemProdutoDTO>>(response);
        }
    }
}
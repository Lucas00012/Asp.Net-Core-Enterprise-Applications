using Microsoft.Extensions.Options;
using NSE.WebApp.MVC.Configuration;
using NSE.WebApp.MVC.Models;

namespace NSE.WebApp.MVC.Services
{
    public class CatalogoService : Service, ICatalogoService
    {
        private readonly HttpClient _httpClient;
        private readonly BaseUrlsConfiguration _baseUrlsConfiguration;

        public CatalogoService(HttpClient httpClient, IOptions<BaseUrlsConfiguration> baseUrlsConfiguration)
        {
            _baseUrlsConfiguration = baseUrlsConfiguration.Value;
            _httpClient = httpClient;

            httpClient.BaseAddress = new Uri(_baseUrlsConfiguration.ApiCatalogoUrl);
        }

        public async Task<ProdutoViewModel> ObterPorId(Guid id)
        {
            var response = await _httpClient.GetAsync($"/catalogo/produtos/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<ProdutoViewModel>(response);
        }

        public async Task<IEnumerable<ProdutoViewModel>> ObterTodos()
        {
            var response = await _httpClient.GetAsync("/catalogo/produtos/");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<IEnumerable<ProdutoViewModel>>(response);
        }
    }
}

using NSE.Catalogo.API.Data;
using NSE.Catalogo.API.Data.Repository;
using NSE.Catalogo.API.Models;
using NSE.Catalogo.API.Services;
using NSE.MessageBus;
using NSE.WebAPI.Core.Usuario;

namespace NSE.Catalogo.API.Infrastructure.StartupConfig
{
    public static class DependencyInjectionConfigExtensions
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<CatalogoContext>();

            services.AddMessageBus(configuration.GetConnectionString("MessageBus"))
                .AddHostedService<CatalogoIntegrationHandler>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();
        }
    }
}

using NSE.MessageBus;
using NSE.Pagamentos.API.Data.Repository;
using NSE.Pagamentos.API.Models;
using NSE.Pagamentos.API.Services;
using NSE.Pagamentos.CardAntiCorruption;
using NSE.Pagamentos.Facade;
using NSE.WebAPI.Core.Usuario;

namespace NSE.Pagamentos.API.Infrastructure.StartupConfig
{
    public static class DependencyInjectionConfigExtensions
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            var pagamentoConfigSection = configuration.GetSection("PagamentoConfig");
            services.Configure<PagamentoConfiguration>(pagamentoConfigSection);

            services.AddScoped<IPagamentoService, PagamentoService>();
            services.AddScoped<IPagamentoFacade, PagamentoCartaoCreditoFacade>();

            services.AddScoped<IPagamentoRepository, PagamentoRepository>();

            services.AddMessageBus(configuration.GetConnectionString("MessageBus"))
                .AddHostedService<PagamentoIntegrationHandler>();
        }
    }
}

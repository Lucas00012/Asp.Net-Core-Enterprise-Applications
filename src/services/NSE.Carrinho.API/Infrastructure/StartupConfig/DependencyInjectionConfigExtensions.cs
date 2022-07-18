using NSE.Carrinho.API.Data;
using NSE.Carrinho.API.Services;
using NSE.MessageBus;
using NSE.WebAPI.Core.Usuario;

namespace NSE.Carrinho.API.Infrastructure.StartupConfig
{
    public static class DependencyInjectionConfigExtensions
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            services.AddMessageBus(configuration.GetConnectionString("MessageBus"))
                .AddHostedService<CarrinhoIntegrationHandler>();
        }
    }
}

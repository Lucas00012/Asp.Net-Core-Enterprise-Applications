using EasyNetQ;
using NSE.Identidade.API.Services;
using NSE.MessageBus;

namespace NSE.Identidade.API.Infrastructure.StartupConfig
{
    public static class DependencyInjectorConfigExtensions
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<JwtTokenService>();

            services.AddMessageBus(configuration.GetConnectionString("MessageBus"));
        }
    }
}

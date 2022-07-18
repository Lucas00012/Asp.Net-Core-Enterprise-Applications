using EasyNetQ;
using MediatR;
using NSE.Clientes.API.Data;
using NSE.Clientes.API.Data.Repository;
using NSE.Clientes.API.Models;
using NSE.Clientes.API.Services;
using NSE.Core.Mediator;
using NSE.MessageBus;
using NSE.WebAPI.Core.Usuario;

namespace NSE.Clientes.API.Infrastructure.StartupConfig
{
    public static class DependencyInjectionConfigExtensions
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(typeof(Startup));

            services.AddScoped<IMediatorHandler, MediatorHandler>();

            services.AddScoped<IClienteRepository, ClienteRepository>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            services.AddMessageBus(configuration.GetConnectionString("MessageBus"))
                .AddHostedService<RegistroClienteIntegrationHandler>();
        }
    }
}

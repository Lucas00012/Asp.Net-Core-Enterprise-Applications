using MediatR;
using NSE.Core.Mediator;
using NSE.MessageBus;
using NSE.Pedidos.API.Application.Queries;
using NSE.Pedidos.API.Services;
using NSE.Pedidos.Domain;
using NSE.Pedidos.Domain.Pedidos;
using NSE.Pedidos.Infra.Data.Repository;
using NSE.WebAPI.Core.Usuario;

namespace NSE.Pedidos.API.Infrastructure.StartupConfig
{
    public static class DependencyInjectionConfigExtensions
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            // API
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            services.AddMessageBus(configuration.GetConnectionString("MessageBus"))
                .AddHostedService<PedidoIntegrationHandler>()
                .AddHostedService<PedidoOrquestradorIntegrationHandler>();

            // Application
            services.AddMediatR(typeof(Startup));
            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddScoped<IVoucherQueries, VoucherQueries>();
            services.AddScoped<IPedidoQueries, PedidoQueries>();

            // Data
            services.AddScoped<IPedidoRepository, PedidoRepository>();
            services.AddScoped<IVoucherRepository, VoucherRepository>();
        }
    }
}

using NSE.WebApp.MVC.Common.User;
using NSE.WebApp.MVC.Configuration;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Services;
using NSE.WebApp.MVC.Services.Handlers;
using Polly;

namespace NSE.WebApp.MVC.Infrastructure.StartupConfig
{
    public static class DependendcyInjectionConfigExtensions
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

            services.AddHttpClient<IAutenticacaoService, AutenticacaoService>();

            services.AddHttpClient<ICatalogoService, CatalogoService>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                .AddPolicyHandler(PollyExtensions.EsperarTentar())
                .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, AspNetUser>();

            var baseUrlsConfigSection = configuration.GetSection("BaseUrlsConfig");
            var baseUrlsConfig = baseUrlsConfigSection.Get<BaseUrlsConfiguration>();
            services.Configure<BaseUrlsConfiguration>(baseUrlsConfigSection);

            #region Refit

            //services.AddHttpClient("Refit", options =>
            //{
            //    options.BaseAddress = new Uri(baseUrlsConfig.ApiCatalogoUrl);
            //})
            //.AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
            //.AddTypedClient(Refit.RestService.For<ICatalogoServiceRefit>);

            #endregion
        }
    }
}

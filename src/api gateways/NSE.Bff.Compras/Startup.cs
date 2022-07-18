using NSE.Bff.Compras.Infrastructure.StartupConfig;
using NSE.WebAPI.Core.Identidade;

namespace NSE.Bff.Compras
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApiConfiguration(Configuration);

            services.AddAuthConfiguration(Configuration);

            services.AddSwaggerConfiguration();

            services.RegisterServices(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwaggerConfiguration();

            app.UseApiConfiguration(env);
        }
    }
}

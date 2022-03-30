using Microsoft.OpenApi.Models;

namespace NSE.Identidade.API.Extensions
{
    public static class SwaggerConfigExtensions
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "NSE - API Identidade",
                    Description = "API para se autenticar na NerdStore"
                });
            });

            services.AddSwaggerGenNewtonsoftSupport();

            return services;
        }

        public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint("/swagger/v1/swagger.json", "NSE - Identidade");

                opt.RoutePrefix = "docs";
            });

            return app;
        }
    }
}

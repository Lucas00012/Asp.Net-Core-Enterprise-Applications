using NSE.Identidade.API.Services;

namespace NSE.Identidade.API.Extensions
{
    public static class ApiConfigExtensions
    {
        public static IServiceCollection AddServicesConfiguration(this IServiceCollection services)
        {
            services.AddScoped<JwtTokenService>();

            return services;
        }

        public static IServiceCollection AddApiConfiguration(this IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson();

            return services;
        }

        public static IApplicationBuilder UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseIdentityConfiguration();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            return app;
        }
    }
}

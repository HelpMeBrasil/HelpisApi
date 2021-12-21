using Donation.API.Configuration;
using Donation.API.Ioc;
using Donation.API.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Donation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddAuthorizationSettings(Configuration);

            // Add data access configuration
            services.AddDataContextConfiguration(Configuration);

            // Add application ioc dependencies
            services.AddApplicationDependencies(Configuration);

            // HTTP client factory
            services.AddHttpClient();

            // Swagger
            services.AddSwaggerConfiguration();

            // Validation settings
            services.AddValidatorConfiguration();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.ConfigureSwagger();
            }

            app.UseCors(option => option.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseRouting();

            // use global exception handler
            app.UseMiddleware<ExceptionMiddleware>();

            // Use authorization
            app.UseAuthorizationSettings();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using TinkrCommon.MassTransit;
using TinkrCommon.MongoDB;
using TinkrCommon.Settings;
using TinkrItemCatalogService.Entities;

namespace TinkrItemCatalogService
{
    public class Startup
    {
        private const string AllowedOriginSetting = "AllowedOrigin"; 
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var serviceSettings = Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();

            services.AddMongo().AddMongoRepository<Item>("items")
                .AddMassTransitWithRabbitMQ();
           
            services.AddControllers(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "TinkrItemCatalogService", Version = "v1"});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TinkrItemCatalogService v1"));
                app.UseCors(builder =>
                {
                    builder.WithOrigins(Configuration[AllowedOriginSetting]).AllowAnyHeader().AllowAnyMethod();
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
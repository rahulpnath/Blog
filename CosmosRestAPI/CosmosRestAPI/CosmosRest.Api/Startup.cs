using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CosmosRest.Domain;
using CosmosRest.Infrastructure;
using CosmosRest.Infrastructure.LOR.Forms.Infrastructure.Cosmos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CosmosRest.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers(o => o.Conventions.Add(new ControllerRouteConvention()))
                .ConfigureApplicationPartManager(m =>
                    m.FeatureProviders.Add(new ControllerFeatureProvider()));


            var cosmosConfig = Configuration.GetSection("Cosmos").Get<CosmosConfiguration>();
            services.AddSingleton(cosmosConfig);
            services.AddSingleton<ICosmosClientFactory, CosmosClientFactory>();
            services.AddScoped(typeof(IRepository<>), typeof(CosmosRepository<>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
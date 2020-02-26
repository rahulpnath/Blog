using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PdfNetCore.Infrastructure;
using PdfNetCore.PdfTemplates;
using PdfNetCore.Services;
using RazorLight;

namespace PdfNetCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            RootPath = webHostEnvironment.ContentRootPath;
        }

        public string RootPath { get; set; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            var engine = new RazorLightEngineBuilder()
                //.UseEmbeddedResourcesProject(typeof(PdfTemplatesPlaceholder))
                .UseFileSystemProject($"{RootPath}/PdfTemplates")
                .UseMemoryCachingProvider()
                .Build();

            services.AddSingleton<IRazorLightEngine>(engine);

            var nrecoConfig = Configuration.GetSection(nameof(NRecoConfig)).Get<NRecoConfig>();
            services.AddSingleton(nrecoConfig);
            services.AddTransient<IPdfGeneratorService, PdfGeneratorService>();
            services.AddTransient<IHtmlGenerationService, HtmlGenerationService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

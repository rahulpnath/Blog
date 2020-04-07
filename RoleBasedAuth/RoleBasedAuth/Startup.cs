using Azure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RoleBasedAuth
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
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddAuthorization(options =>
            {
                var adGroups = GetAdGroups();

                foreach (var adGroup in adGroups)
                    options.AddPolicy(
                        adGroup.GroupName,
                        policy =>
                            policy.AddRequirements(new IsMemberOfGroupRequirement(adGroup.GroupName, adGroup.GroupId)));
            });


            services.AddSingleton<IAuthorizationHandler, IsMemberOfGroupHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
        }

        public static List<AdGroupConfig> GetAdGroups()
        {
            var client = GetGraphApiClient().Result;
            var allAdGroups = new List<AdGroupConfig>();

            var groups = client.Groups.Request().GetAsync().Result;

            while (groups.Count > 0)
            {
                allAdGroups.AddRange(groups.Select(a => new AdGroupConfig() { GroupId = a.Id, GroupName = a.DisplayName }));

                if (groups.NextPageRequest != null)
                    groups = groups.NextPageRequest.GetAsync().Result;
                else
                    break;
            }

            return allAdGroups;
        }

        private static async Task<GraphServiceClient> GetGraphApiClient()
        {
            var credential = new DefaultAzureCredential();
            var token = await credential.GetTokenAsync(
                new Azure.Core.TokenRequestContext(
                    new[] { "https://graph.microsoft.com/.default" }));
            var accessToken = token.Token;
            var graphServiceClient = new GraphServiceClient(
                new DelegateAuthenticationProvider((requestMessage) =>
                {
                    requestMessage
                    .Headers
                    .Authorization = new AuthenticationHeaderValue("bearer", accessToken);

                    return Task.CompletedTask;
                }));

            return graphServiceClient;
        }

    }
}

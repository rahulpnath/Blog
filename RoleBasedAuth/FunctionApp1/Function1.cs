using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Services.AppAuthentication;
using System.Net.Http.Headers;

namespace FunctionApp1
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                log.LogInformation("C# HTTP trigger function processed a request.");

                var target = "<tenantId>";
                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                string accessToken = await azureServiceTokenProvider.GetAccessTokenAsync(target);
                log.LogInformation("Token: {0}", accessToken);
                
                var wc = new System.Net.Http.HttpClient();
                wc.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var result = await wc.GetAsync("https://rolebasedauthtest.azurewebsites.net/api/values");

                log.LogInformation(result.StatusCode.ToString());
                var content = await result.Content.ReadAsStringAsync();
                log.LogInformation(content);
            }
            catch(Exception e)
            {
                log.LogError(e.Message);
            }
        }
    }
}

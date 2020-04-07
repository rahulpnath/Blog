using Microsoft.Graph;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RoleBasedAuth
{
    public static class ADHelper
    {
        private static async Task<GraphServiceClient> GetGraphApiClient(string clientId, string secret, string domain)
        {
            var credentials = new ClientCredential(clientId, secret);
            var authContext =
                new AuthenticationContext($"https://login.microsoftonline.com/{domain}/");
            var token = await authContext
                .AcquireTokenAsync("https://graph.microsoft.com/", credentials);

            var graphServiceClient = new GraphServiceClient(new DelegateAuthenticationProvider((requestMessage) =>
            {
                requestMessage
                    .Headers
                    .Authorization = new AuthenticationHeaderValue("bearer", token.AccessToken);

                return Task.CompletedTask;
            }));

            return graphServiceClient;
        }

        public async static Task<string[]> GetADGroups()
        {
            var client = await GetGraphApiClient("", "", "");

            var result = await client.Groups.Request().GetAsync();

            return result.Select(a => a.Id).ToArray();
        }

    }
}

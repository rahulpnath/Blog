using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using SqlMSI.Controllers;
using System.Data.SqlClient;

namespace SqlMSI
{
    public class QuoteContext : DbContext
    {
        public QuoteContext(DbContextOptions options) : base(options)
        {
            var conn = (Microsoft.Data.SqlClient.SqlConnection)Database.GetDbConnection();
            var opt = new DefaultAzureCredentialOptions() { ExcludeSharedTokenCacheCredential = true };
            var credential = new DefaultAzureCredential(opt);
            var token = credential
                    .GetToken(new Azure.Core.TokenRequestContext(
                        new[] { "https://database.windows.net/.default" }));
            conn.AccessToken = token.Token;
        }

        public DbSet<Quote> Quote { get; set; }
    }
}

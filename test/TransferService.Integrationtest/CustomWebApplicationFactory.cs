using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace TransferService.Integrationtest
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:PayphoneConnection"] =
                        "Server=(localdb)\\MSSQLLocalDB;Database=PAYPHONEWALLETS_TEST;Trusted_Connection=True;TrustServerCertificate=True;"
                });
            });
        }
    }
}

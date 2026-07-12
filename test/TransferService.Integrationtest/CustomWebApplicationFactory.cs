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
                var settings = config.Build();

                var conn = settings.GetConnectionString("PayphoneConnection");

                conn = conn.Replace(
                    "Database=PAYPHONEWALLETS;",
                    "Database=PAYPHONEWALLETS_TEST;");


                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:PayphoneConnection"] = conn

                });
            });
        }
    }
}

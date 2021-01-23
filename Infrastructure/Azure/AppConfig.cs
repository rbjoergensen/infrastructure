using System;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Configuration;
using Azure.Identity;
using System.Security.Cryptography.X509Certificates;
using System.IO;

namespace Infrastructure.Azure
{
    class AppConfig
    {
        public static IConfiguration config;

        public static void Load(string uri, string tenantId, string clientId, string labelFilter, string pfxFilePath = null, string certPassword = null)
        {
            try
            {
                var endpoint = new Uri(uri);
                var builder = new ConfigurationBuilder();

                builder.AddAzureAppConfiguration(options =>
                {
                    var clientAssertionCertPfx = Certificates.LoadX509Certificate(pfxFilePath, certPassword);
                    options.Connect(new Uri(uri), new ClientCertificateCredential(tenantId, clientId, clientAssertionCertPfx)).ConfigureRefresh(refresh =>
                    {
                        // Use a sentinal to avoid splicing
                        refresh.Register(KeyFilter.Any, refreshAll: true).SetCacheExpiration(TimeSpan.FromSeconds(10));
                        // Or use a keyFilter string
                        // refresh.Register("this:is:my:config", refreshAll: true).SetCacheExpiration(TimeSpan.FromSeconds(10));
                    }).Select(KeyFilter.Any, labelFilter)
                    .ConfigureKeyVault(kv => // Configure access to keyvaults using the same application
                    {
                        kv.SetCredential(new ClientCertificateCredential(tenantId, clientId, clientAssertionCertPfx));
                    });
                });

                config = builder.Build();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading AppConfig: {ex.Message}");
            }
        }
    }
}

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
            Logging.log.Information("Running LoadAppConfig Method");
            try
            {
                // Using a X509 certificate to access configuration by authenticating through an application with the 'App Configuration Data Reader' role on the App Configuration
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
                Logging.log.Error(ex.Message);
            }
        }
    }
}

using System;
using System.Security.Cryptography.X509Certificates;

namespace Infrastructure
{
    class Certificates
    {
        public static X509Certificate2 LoadX509Certificate(string certPath = null, string certPass = null)
        {
            X509Store store = new X509Store();
            try
            {
                // Set storelocation to My when using on Windows and Root when running on Linux(eg. Docker container)
                store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection col = new X509Certificate2Collection();
                col.Import(certPath, certPass, X509KeyStorageFlags.PersistKeySet);
                if (col == null || col.Count == 0)
                {
                    Logging.log.Error("Problem loading pfx certificate");
                }
                return col[0];
            }
            catch (Exception ex)
            {
                Logging.log.Error($"Problem loading pfx certificate: {ex.Message}");
                return null;
            }
            finally
            {
                store.Close();
            }
        }
    }
}

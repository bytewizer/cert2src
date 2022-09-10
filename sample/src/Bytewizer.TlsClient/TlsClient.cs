using System;
using System.Net;
using System.Text;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

using Bytewizer.TlsClient.Properties;

namespace Bytewizer.TlsClient
{
    internal partial class TlsClient
    {
        private readonly X509Certificate[] certx509;

        public TlsClient()
        {
            var certificate = Resources.GetBytes(Resources.BinaryResources.Certificate);
            certx509 = new X509Certificate[] { new X509Certificate(certificate) };
        }

        public bool Connect(string url)
        {
            int total = 0;
            byte[] result = new byte[1024];

            try
            {
                using (var req = HttpWebRequest.Create(url) as HttpWebRequest)
                {
                    req.KeepAlive = false;
                    req.HttpsAuthentCerts = certx509;
                    req.ReadWriteTimeout = 2000;

                    using (var res = req.GetResponse() as HttpWebResponse)
                    {
                        using (var stream = res.GetResponseStream())
                        {
                            int read;
                            var builder = new StringBuilder();
                            do
                            {
                                read = stream.Read(result, 0, result.Length);
                                total += read;

                                builder.Append(Encoding.UTF8.GetChars(result, 0, read));
                            }
                            while (read != 0);

                            Debug.WriteLine($"Response : {builder}");
                        }
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
using System.Net;
using System.Text;
using System.Reflection;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Bytewizer.Commandline
{
    using Parameters = Dictionary<string, Action<string>>;

    public static class Cert2Src
    {
        private static int _width = 18;
        private static string _namespace = null;
        private static string _classname = null;
        private static bool _code = false;
        private static string? _path = null;

        public static int Main(string[] args)
        {
            var parameters = new Parameters {
                {"--help", x => ShowHelp()},
                {"--path", x => _path = x ??= AppDomain.CurrentDomain.BaseDirectory},
                {"--code", _ => _code = true},
                {"--width", x => _width = int.Parse(x)},
                {"--namespace", x => _namespace = x},
                {"--classname", x => _classname = x}
            };

            if (args.Length == 0 || args[0].Equals("/?") || args[0].Equals("--help"))
            {
                ShowHelp();
            }

            if (!Uri.TryCreate(args[0], UriKind.Absolute, out Uri? url))
            {
                Console.Error.WriteLine($"Url must be an absolute fully quilified doman name.");
                Environment.Exit(1);
            }

            if (url.Scheme != "https")
            {
                Console.Error.WriteLine($"Url must be an absolute fully quilified doman name staring with 'https://'");
                Environment.Exit(1);
            }

            Parse(parameters, args);

            try
            {
                Run(url);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Environment.Exit(1);
            }

            return Environment.ExitCode;
        }

        private static void Run(Uri url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);

            request.AllowAutoRedirect = false;
            request.ServerCertificateValidationCallback = ServerCertificateValidationCallback;

            var response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Console.WriteLine($"Root certificate downloaded from '{url}'");
            }
            else
            {
                Console.Error.WriteLine($"Response status '{response.StatusDescription.ToLower()}' downloading root certificate from '{url}'");
                Environment.Exit(1);
            }

            response.Close();
        }

        private static bool ServerCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            var chainRoot = chain.ChainElements[chain.ChainElements.Count - 1].Certificate;

            if (_path == null)
            {
                if (_code == false)
                {
                    var cert = ExportToPem(chainRoot);
                    Console.WriteLine(cert);
                }
                else
                {
                    var cert = ExportToCsharp(chainRoot);
                    Console.WriteLine(cert);
                }
            }
            else
            {
                if (_code == false)
                {
                    var cert = ExportToPem(chainRoot);
                    Console.WriteLine(cert);

                    SaveCertificate(_path, "certificate.crt", cert);
                }
                else
                {
                    var cert = ExportToCsharp(chainRoot);
                    Console.WriteLine(cert);

                    SaveCertificate(_path, "certificate.cs", cert);
                }
            }

            return true;
        }

        private static void SaveCertificate(string path, string filename, string cert)
        {
            var fullPath = path;

            if (!Path.HasExtension(path))
            {
                fullPath = Path.Combine(path, filename);
            }

            try
            {
                File.WriteAllText(fullPath, cert);
            }
            catch(Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Environment.Exit(1);
            }

            Console.WriteLine($"Root certificate successfully exported to '{fullPath}'");
        }

        public static string ExportToPem(X509Certificate2 cert)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("-----BEGIN CERTIFICATE-----");
            builder.AppendLine(Convert.ToBase64String(cert.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks));
            builder.AppendLine("-----END CERTIFICATE-----");

            return builder.ToString();
        }

        public static string ExportToCsharp(X509Certificate2 cert)
        {
            StringBuilder builder = new StringBuilder();

            var content = ExportToPem(cert);

            if (_namespace != null)
            {
                builder.AppendLine(_namespace);
                builder.AppendLine("{");
            }

            if (_classname != null)
            {
                builder.AppendLine(_classname);
                builder.AppendLine("{");
            }

            builder.AppendLine("private static readonly byte[] Certificate = ");
            builder.AppendLine("{");

            int i = 0;
            foreach (char ch in content)
            {
                if (i % _width == 0)
                {
                    if (i != 0)
                    {
                        builder.AppendLine();
                    }
                    builder.Append("     ");
                }

                builder.Append($"0x{Convert.ToByte(ch):x2}");

                if (i < content.Length - 1)
                {
                    builder.Append(", ");
                }

                i++;
            }

            builder.AppendLine();
            builder.AppendLine("};");

            if (_namespace != null)
            {
                builder.AppendLine("}");

                if (_classname == null)
                {
                    builder.Append(";");
                }
            }

            if (_classname != null)
            {
                builder.AppendLine("};");
            }

            return builder.ToString();
        }
        private static void Parse(Parameters parameters, string[] args)
        {
            Action<string> currentCallback = null;

            foreach (var arg in args)
            {
                if (arg == args[0])
                {
                    continue;
                }

                if (parameters.TryGetValue(arg, out var callback))
                {
                    currentCallback?.Invoke(null);
                    currentCallback = callback;
                }
                else if (currentCallback != null)
                {
                    currentCallback(arg);
                    currentCallback = null;
                }
                else
                {
                    Console.Error.WriteLine($"Unknown command: '{arg}'");
                    Environment.Exit(1);
                }
            }

            currentCallback?.Invoke(null);
        }

        private static void ShowHelp()
        {
            var name = AppDomain.CurrentDomain.FriendlyName;
            var version = Assembly.GetEntryAssembly()?.GetName().Version;

            Console.WriteLine($"{name} Version: {version}");
            Console.WriteLine($"Usage: {name}.exe url [options]");
            Console.WriteLine();
            Console.WriteLine("Download and export root certificates required for TinyCLR OS to access secure sites.");
            Console.WriteLine();
            Console.WriteLine("options:");
            Console.WriteLine(" --help        Displays general help information about other commands.");
            Console.WriteLine(" --path        Output chain root certificate as base-64 encoded PEM format to file.");
            Console.WriteLine(" --code        Set output format as csharp source code array.");
            Console.WriteLine(" --width       Width of the source code array output (default 18).");
            Console.WriteLine(" --namespace   Namespace used when creating code array output.");
            Console.WriteLine(" --classname   Class name structure used when creating code array output.");
            Console.WriteLine();

            Environment.Exit(0);
        }
    }
}

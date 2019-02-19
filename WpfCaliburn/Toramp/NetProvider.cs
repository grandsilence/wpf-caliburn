using System;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using Leaf.Http;

namespace WpfCaliburn.Toramp
{
    public abstract class NetProvider : IDisposable
    {
        protected readonly HttpAgent Http;
        protected readonly CookieContainer Cookies;
        public readonly Uri BaseUri;

        protected NetProvider()
        {
            Cookies = new CookieContainer();
            var httpHandler = new HttpClientHandler {
                AllowAutoRedirect = true,
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
                CookieContainer = Cookies,
                UseCookies = true,
                SslProtocols = SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12,
                #if DEBUG
                Proxy = new WebProxy("http://127.0.0.1:8887"),
                #endif
            };

            Http = new HttpAgent(httpHandler, true) {
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/71.0.3578.98 Safari/537.36",
                AcceptLanguage = "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7"
            };

            Http.DefaultRequestHeaders.Add("Connection", "keep-alive");
        }

        protected NetProvider(string baseUrl) : this()
        {
            Http.BaseAddress = BaseUri = new Uri(baseUrl);
            Http.Origin = baseUrl;
        }

        public void Dispose()
        {
            Http?.Dispose();
        }
    }
}
using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;

namespace WebProxyServiceNS.Test
{
    class Program
    {
        private static WebProxyService proxyService = new WebProxyService();
        private static HttpClient httpClient = new HttpClient(new HttpClientHandler { UseProxy = true, Proxy = proxyService });

        static async Task Main(string[] args)
        {
            var proxies = new[] { 
                null,
                new WebProxy("104.238.172.20", 8080),
                new WebProxy("104.238.167.193", 8080),
                new WebProxy("136.244.102.38", 8080),
                new WebProxy("95.179.202.40", 8080)
            };

            foreach (var proxy in proxies)
            {
                var ip = await GetIpWithProxyAsync(proxy);
                Console.WriteLine($"Proxy: {(proxy == null ? "null" : $"{proxy.Address.Host}:{proxy.Address.Port}")}, Ip: {ip}");
            }
        }

        private static async Task<string> GetIpWithProxyAsync(IWebProxy proxy)
        {
            proxyService.Proxy = proxy;
            return await httpClient.GetStringAsync("https://api.ipify.org/");
        }
    }
}

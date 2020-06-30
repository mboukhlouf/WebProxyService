using BenchmarkDotNet.Attributes;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebProxyServiceNS.Benchmark
{
    [MemoryDiagnoser]
    public class WebProxyServiceBenchmark
    {
        private static HttpClientHandler handler1 = new HttpClientHandler();
        private static HttpClient client1 = new HttpClient(handler1);
        
        private static WebProxyService proxyService = new WebProxyService();
        private static HttpClientHandler handler2 = new HttpClientHandler { UseProxy = true, Proxy = proxyService };
        private static HttpClient client2 = new HttpClient(handler2);


        public WebProxyServiceBenchmark()
        {
            SetupAsync().GetAwaiter().GetResult();
        }

        private async Task SetupAsync()
        {
            await client1.GetStringAsync("https://api.ipify.org/");
            await client2.GetStringAsync("https://api.ipify.org/");
        }

        [Benchmark(Baseline = true)]
        public async Task ChangeProxy1()
        {
            var newProxy = WebRequest.DefaultWebProxy;
            try
            {
                handler1.Proxy = newProxy;
            }
            catch (InvalidOperationException)
            {
                var cookieContainer = handler1.CookieContainer;
                var timeout = client1.Timeout;

                client1.Dispose();
                handler1.Dispose();

                handler1 = new HttpClientHandler()
                {
                    UseCookies = true,
                    AllowAutoRedirect = false,
                    Proxy = newProxy,
                    CookieContainer = cookieContainer
                };

                client1 = new HttpClient(handler1)
                {
                    Timeout = timeout
                };
            }

            _ = await client1.GetStringAsync("https://api.ipify.org/");
        }

        [Benchmark]
        public async Task ChangeProxy2()
        {
            var newProxy = WebRequest.DefaultWebProxy;
            proxyService.Proxy = newProxy;

            _ = await client2.GetStringAsync("https://api.ipify.org/");
        }
    }
}

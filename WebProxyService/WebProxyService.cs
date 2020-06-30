using System;
using System.Net;

namespace WebProxyServiceNS
{
    public class WebProxyService : IWebProxy
    {
        private IWebProxy proxy;

        public IWebProxy Proxy
        {
            get => proxy ?? (proxy = WebRequest.DefaultWebProxy);
            set => proxy = value;
        }

        public ICredentials Credentials
        {
            get => Proxy.Credentials;
            set => Proxy.Credentials = value;
        }

        public WebProxyService()
        {
        }

        public WebProxyService(IWebProxy proxy)
        {
            Proxy = proxy;
        }

        public Uri GetProxy(Uri destination)
            => Proxy.GetProxy(destination);

        public bool IsBypassed(Uri host)
            => Proxy.IsBypassed(host);
    }
}

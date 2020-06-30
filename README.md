# WebProxyService
A class to solve the issue of not being able to change Proxy after HttpClient has started in .NET.

I was having this issue with HttpClient in .NET. You set the proxy to the HttpClientHandler. Once the client sends one request, you won't be able to change the Proxy property of the handler or you get InvalidOperationException. So the solution would be instantiating a new HttpClient with the new proxy, but it has high startup cost.

But this solution is very simple and not as costly, Proxy property of HttpClientHandler takes an object that implements IWebProxy. IWebProxy interface has a method GetProxy that return the Uri of the proxy. So WebProxyService implements this interface and it has a property Proxy which is the wrapped proxy, and when GetProxy is called, it calls GetProxy of the proxy inside it.

Solution 1:

``` csharp
try
{
    clientHandler.Proxy = newProxy;
}
catch (InvalidOperationException)
{
    var cookieContainer = clientHandler.CookieContainer;
    client.Dispose();
    clientHandler.Dispose();
    clientHandler= new HttpClientHandler()
    {
        Proxy = newProxy,
        CookieContainer = cookieContainer
    };
    client= new HttpClient(clientHandler):=;
}
```

Solution 2:
```csharp
// var proxyService = new WebProxyService();
// clientHandler.Proxy = proxyService;

proxyService.Proxy = newProxy;
```

Benchmark:
```
|       Method |     Mean |   Error |   StdDev |   Median | Ratio | RatioSD | Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------- |---------:|--------:|---------:|---------:|------:|--------:|------:|------:|------:|----------:|
| ChangeProxy1 | 496.7 ms | 6.88 ms |  5.75 ms | 494.3 ms |  1.00 |    0.00 |     - |     - |     - |   33.3 KB |
| ChangeProxy2 | 129.4 ms | 4.86 ms | 13.46 ms | 123.5 ms |  0.27 |    0.03 |     - |     - |     - |   4.18 KB |
```

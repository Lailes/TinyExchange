using System;
using System.Net.Http;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using TinyExchange.RazorPages;

namespace TinyExchange.Tests.Environment;

public class WebAppTestHost
{
    private TestServer _testServer;
    public IServiceProvider Services => _testServer.Host.Services;

    public void Start() =>
        _testServer = new TestServer(WebHost
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration((_, configurationBuilder) =>
            {
                configurationBuilder.AddCommandLine(
                    new [] { "--port", "5432", "--host", "127.0.0.1", "--db", "tiny_exchange_db", "--username", "nikita", "--password", "nikita2209" });
            })
            .UseStartup<Startup>());

    public HttpClient Client => _testServer.CreateClient();

    public void Dispose() => _testServer?.Dispose();
}
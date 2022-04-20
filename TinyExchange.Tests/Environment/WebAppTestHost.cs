using System;
using System.Net.Http;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TinyExchange.RazorPages;
using TinyExchange.RazorPages.Database;
using TinyExchange.Tests.Environment.Moqs;

namespace TinyExchange.Tests.Environment;

public class WebAppTestHost
{
    private TestServer _testServer;
    public IServiceProvider Services => _testServer.Host.Services;

    public void Start()
    {
        var builder = WebHost.CreateDefaultBuilder();
        builder.UseStartup<Startup>();

        builder.ConfigureServices(services =>
            services.Replace(
                new ServiceDescriptor(
                    typeof(IApplicationContext), 
                    _ => new MoqContext(new DbContextOptionsBuilder<MoqContext>().UseInMemoryDatabase("TEST_DB").Options), 
                    ServiceLifetime.Transient)));
        
        _testServer = new TestServer(builder);
    }

    public HttpClient GetClient() => _testServer.CreateClient();

    public void Dispose() => _testServer?.Dispose();
}
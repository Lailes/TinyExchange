using Microsoft.Extensions.DependencyInjection;
using TinyExchange.RazorPages.Database;
using TinyExchange.Tests.Environment.Moqs;

namespace TinyExchange.Tests.Environment;

public class WebAppTestEnvironment
{
    public WebAppTestHost WebAppHost { get; }

    public WebAppTestEnvironment() => WebAppHost = new WebAppTestHost();

    public void Start() => WebAppHost.Start();

    public void Prepare()
    {
        var context = (MoqContext) WebAppHost.Services.GetRequiredService(typeof(IApplicationContext));
        context.Reset();
    }

    public void Clear()
    {
    }

    public void Dispose() => WebAppHost?.Dispose();
}
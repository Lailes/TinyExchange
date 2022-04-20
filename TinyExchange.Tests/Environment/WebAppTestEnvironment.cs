using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TinyExchange.RazorPages.Database;
using TinyExchange.Tests.Environment.Extensions;

namespace TinyExchange.Tests.Environment;

public class WebAppTestEnvironment
{
    public WebAppTestHost WebAppHost { get; }

    public WebAppTestEnvironment() => WebAppHost = new WebAppTestHost();

    private ApplicationContext ApplicationContext =>
        (ApplicationContext) WebAppHost.Services.GetRequiredService(typeof(ApplicationContext));

    public void Start() => WebAppHost.Start();

    public async Task PrepareAsync() => await ApplicationContext.SeedAsync();

    public async Task ClearAsync() => await ApplicationContext.ClearAsync();

    public void Dispose() => WebAppHost?.Dispose();
}
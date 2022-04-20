using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TinyExchange.Tests.Environment;

[TestFixture]
public class TestBase
{
    protected WebAppTestEnvironment Environment { get; set; }

    public HttpClient Client { get; private set; }
    
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        Environment = new WebAppTestEnvironment();
        Environment.Start();
    }
    
    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        Environment.Dispose();
    }

    [SetUp]
    public async Task TearUp()
    {
        Environment.Prepare();
        Client = Environment.WebAppHost.GetClient();
    }

    [TearDown]
    public async Task TearDown()
    {
        Environment.Clear();
    }
}
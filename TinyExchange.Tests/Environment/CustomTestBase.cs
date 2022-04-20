using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using FluentAssertions;
using Microsoft.Net.Http.Headers;
using NUnit.Framework;

namespace TinyExchange.Tests.Environment;

[TestFixture]
public class CustomTestBase
{
    protected WebAppTestEnvironment Environment { get; private set; }

    protected HttpClient Client { get; private set; }
    protected HttpClient KycClient { get; private set; }
    protected HttpClient FundsClient { get; private set; }
    protected HttpClient AdminClient { get; private set; }
    
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        Environment = new WebAppTestEnvironment();
        Environment.Start();
    }
    
    [OneTimeTearDown]
    public void OneTimeTearDown() => Environment.Dispose();

    [SetUp]
    public async Task TearUp()
    {
        await Environment.PrepareAsync();
        Client = Environment.WebAppHost.Client;
        KycClient = await GetAuthorizedClient("kyc", "1");
        FundsClient = await GetAuthorizedClient("funds", "1");
        AdminClient = await GetAuthorizedClient("admin", "1");
    }

    [TearDown]
    public async Task TearDown()
    {
        await Environment.ClearAsync();
        Client.Dispose();
        KycClient.Dispose();
        FundsClient.Dispose();
        AdminClient.Dispose();
    }


    private async Task<HttpClient> GetAuthorizedClient(string email, string password)
    {
        var client = Environment.WebAppHost.Client;
        var response = await client.PostAsync("/log-in", 
            new FormUrlEncodedContent(
                new Dictionary<string, string> { {"email", email}, {"password", password} }));

        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        client.DefaultRequestHeaders.Add(HeaderNames.Cookie, response.Headers.GetValues(HeaderNames.SetCookie));
        return client;
    }
}
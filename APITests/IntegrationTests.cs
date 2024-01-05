using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using SamplingAPI;

namespace APITests;

public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    internal static readonly string Url = "/api/Estimator";

    public IntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task SRSEndpointIsAlive()
    {
        string body = $$"""
            {
                "targetColumn": "age",
                "withReplacement": true,
                "populationSize": 50,
                "data": {
                    "age": [
                        9, 10, 11, 18, 22
                    ]
                },
                "significanceLevel": 5
            }
            """;
        HttpContent content = Helpers.GetJSONContent(body);
        var response = await _client.PostAsync($"{Url}/srs", content);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
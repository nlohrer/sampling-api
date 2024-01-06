using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using SamplingAPI;
using System.Text;

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

    [Fact]
    public async Task ModelEndpointIsAlive()
    {
        string body = $$"""
        {
            "targetColumn": "age",
            "auxiliaryColumn": "height",
            "auxiliaryMean": 15,
            "populationSize": 5,
            "data": {
              "age": [
                9, 10, 11
              ],
              "height": [
                11, 11, 11
              ]
            },
            "significanceLevel": 5
        }
        """;
        HttpContent content = Helpers.GetJSONContent(body);
        var response = await _client.PostAsync($"{Url}/model?modelType=diff", content);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        string actual = await response.Content.ReadAsStringAsync();
        string expected = $$$"""
        {"mean":14,"variance":0.13333333333333333,"confidenceInterval":{"lowerBound":13.284309191526583,"upperBound":14.715690808473417,"significanceLevel":5}}
        """;
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task ModelEndpointOnlyAcceptsValidModels()
    {
        string body = "";
        HttpContent content = Helpers.GetJSONContent(body);
        var responseNoModel = await _client.PostAsync($"{Url}/model", content);
        Assert.Equal(HttpStatusCode.BadRequest, responseNoModel.StatusCode);

        var responseInvalidModel = await _client.PostAsync($"{Url}/model?modelType=sum", content);
        Assert.Equal(HttpStatusCode.BadRequest, responseInvalidModel.StatusCode);
    }
}
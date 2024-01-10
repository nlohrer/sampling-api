using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;

namespace APITests;

public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    internal static readonly string EstimatorUrl = "/api/Estimator";
    internal static readonly string SampleUrl = "/api/Sample";
    internal static readonly string SizeUrl = "/api/SampleSize";
    internal static readonly string FormatUrl = "/api/Format";

    public IntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task SRSEndpointIsAlive()
    {
        string body = $$"""
            {
                "data": [
                    9, 10, 11, 18, 22
                ],
                "withReplacement": true,
                "populationSize": 50,
                "significanceLevel": 5
            }
            """;
        HttpContent content = Helpers.GetJSONContent(body);
        var response = await _client.PostAsync($"{EstimatorUrl}/srs", content);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        string actual = await response.Content.ReadAsStringAsync();
        string expected = "{\"mean\":14,\"variance\":6.5,\"confidenceInterval\":{\"lowerBound\":9.002960876679072,\"upperBound\":18.997039123320928,\"significanceLevel\":5}}";
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task ModelEndpointIsAlive()
    {
        string body = $$"""
        {
            "data": [
                9, 10, 11
            ],
            "auxiliaryData": [
                11, 11, 11
            ],
            "auxiliaryMean": 15,
            "populationSize": 5,
            "significanceLevel": 5
        }
        """;
        HttpContent content = Helpers.GetJSONContent(body);
        var response = await _client.PostAsync($"{EstimatorUrl}/model?modelType=diff", content);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        string actual = await response.Content.ReadAsStringAsync();
        string expected = $$$"""
        {"mean":14,"variance":0.13333333333333333,"confidenceInterval":{"lowerBound":13.284309191526583,"upperBound":14.715690808473417,"significanceLevel":5}}
        """;
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task DataLengthAndAuxiliaryDataLengthMustBeEqual()
    {
        string body = $$"""
        {
            "data": [
                9, 10, 11, 10
            ],
            "auxiliaryData": [
                11, 11, 11
            ],
            "auxiliaryMean": 15,
            "populationSize": 5,
            "significanceLevel": 5
        }
        """;
        HttpContent content = Helpers.GetJSONContent(body);
        var response = await _client.PostAsync($"{EstimatorUrl}/model?modelType=diff", content);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task ModelEndpointOnlyAcceptsValidModels()
    {
        string body = "";
        HttpContent content = Helpers.GetJSONContent(body);
        var responseNoModel = await _client.PostAsync($"{EstimatorUrl}/model", content);
        Assert.Equal(HttpStatusCode.BadRequest, responseNoModel.StatusCode);

        var responseInvalidModel = await _client.PostAsync($"{EstimatorUrl}/model?modelType=sum", content);
        Assert.Equal(HttpStatusCode.BadRequest, responseInvalidModel.StatusCode);
    }

    [Fact]
    public async Task DesignEndpointIsAlive()
    {
        string body = $$$"""
        {
            "data": [
                    4, 9, 24
                ],
            "inclusionProbabilities": [
                    0.05, 0.1, 0.125
                ],
            "populationSize": 20,
            "significanceLevel": 5
        }
        """;
        HttpContent content = Helpers.GetJSONContent(body);
        var response = await _client.PostAsync($"{EstimatorUrl}/design", content);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        string actual = await response.Content.ReadAsStringAsync();
        string expected = "{\"mean\":18.1,\"variance\":28.810000000000016,\"confidenceInterval\":{\"lowerBound\":7.5797102701494,\"upperBound\":28.620289729850604,\"significanceLevel\":5}}";
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task DataLengthAndInclusionProbabilitiesLengthMustBeEqual()
    {
        string body = $$$"""
        {
            "data": [
                    4, 9
                ],
            "inclusionProbabilities": [
                    0.05, 0.1, 0.125
                ],
            "populationSize": 20,
            "significanceLevel": 5
        }
        """;
        HttpContent content = Helpers.GetJSONContent(body);
        var response = await _client.PostAsync($"{EstimatorUrl}/design", content);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task SampleSRSEndpointIsAlive()
    {
        string body = $$$"""
        {
            "age": [
                23, 54, 28, 58, 52
            ],
            "height": [
                165, 182, 169, 190, 175
            ]
        }        
        """;
        HttpContent content = Helpers.GetJSONContent(body);
        var response = await _client.PostAsync($"{SampleUrl}/srs?withReplacement=true&n=6", content);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        string responseString = await response.Content.ReadAsStringAsync();
        Dictionary<string, List<double>>? responseObject = JsonConvert.DeserializeObject<Dictionary<string, List<double>>>(responseString);
        Assert.NotNull(responseObject);

        Assert.Equal(6, responseObject.Values.ElementAt(0).Count);
    }

    [Fact]
    public async Task SampleArbitraryData()
    {
        string body = $$$"""
        {
            "age": [20, 34],
            "name": ["Alex", "Angelina"],
            "married": [true, false],
            "lucky numbers": [[1,2],[4]],
            "pet": [{"name": "Fluffy"}, {"name": "Minou", "kind": "cat", "age": 4}]
        }
        """;
        HttpContent content = Helpers.GetJSONContent(body);
        var response = await _client.PostAsync($"{SampleUrl}/srs?withReplacement=true&n=3", content);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        string responseString = await response.Content.ReadAsStringAsync();
        Dictionary<string, List<object>>? responseObject = JsonConvert.DeserializeObject<Dictionary<string, List<Object>>>(responseString);
        Assert.NotNull(responseObject);

        Assert.Equal(5, responseObject.Count);
    }

    [Fact]
    public async Task SamplingRequiresColumnsToBeEquallyLong()
    {
        string body = $$$"""
        {
            "age": [20, 23],
            "name": ["Angelina"]
        }
        """;
        HttpContent content = Helpers.GetJSONContent(body);
        var response = await _client.PostAsync($"{SampleUrl}/srs?withReplacement=true&n=3", content);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task SystematicSamplingEndpointIsAlive()
    {
        string body = $$$"""
        {
            "age": [
                19, 23, 39, 83, 54, 63, 34
            ],
            "name": [
                "Alice", "Bob", "Carol", "Dave", "Erin", "Frank", "Grace"
            ]
        }
        """;
        HttpContent content = Helpers.GetJSONContent(body);
        var response = await _client.PostAsync($"{SampleUrl}/systematic?interval=3", content);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        string responseBody = await response.Content.ReadAsStringAsync();
        string expected = $$"""
        {"age":[19,83,34],"name":["Alice","Dave","Grace"]}
        """;
        Assert.Equal(expected, responseBody);
    }

    [Fact]
    public async Task SampleSizeSRSEndpointIsAlive()
    {
        string body = $$$"""
        {
            "e": 0.015,
            "alpha": 5,
            "withReplacement": true,
            "worstCasePercentage": 0.4
        }
        """;
        HttpContent content = Helpers.GetJSONContent(body);
        var response = await _client.PostAsync($"{SizeUrl}/srs", content);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        string responseBody = await response.Content.ReadAsStringAsync();
        Assert.Equal("4098", responseBody);
    }

    [Fact]
    public async Task StratifiedSampleDistributionEndpointIsAlive()
    {
        string body = $$$"""
        {
            "stratumNames": [
                "b", "m", "s"
            ],
            "sampleSize": 50,
            "stratumTotalSizes": [
                40, 100, 220
            ],
            "stratumVariances": [
                250, 120, 50
            ],
            "stratumCosts": [
                130, 80, 60
            ]
        }
        """;
        HttpContent content = Helpers.GetJSONContent(body);
        var response = await _client.PostAsync($"{SizeUrl}/stratified/distribution", content);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        string responseBody = await response.Content.ReadAsStringAsync();
        string expected = """
            {"b":7,"m":16,"s":27}
            """;
        Assert.Equal(expected, responseBody);
    }

    [Fact]
    public async Task DeterminingSizeWithoutReplacementRequiresSpezifyingPopulationSize()
    {
        string body = $$$"""
        {
            "e": 0.02,
            "alpha": 5,
            "withReplacement": false
        }
        """;
        HttpContent content = Helpers.GetJSONContent(body);
        var response = await _client.PostAsync($"{SizeUrl}/srs", content);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task StratifiedEndpointIsAlive()
    {
        string body = $$$"""
        {
            "data": [
                9, 10, 11, 18, 22, 25
            ],
            "strata": [
                "m", "m", "m", "f", "f", "f"
            ],
            "stratumSizes": {
                "m": 25,
                "f": 75
            },
            "significanceLevel": 5
        }
        """;
        HttpContent content = Helpers.GetJSONContent(body);
        var response = await _client.PostAsync($"{EstimatorUrl}/stratified", content);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        string actual = await response.Content.ReadAsStringAsync();
        string expected = "{\"mean\":18.75,\"variance\":2.2383333333333337,\"confidenceInterval\":{\"lowerBound\":15.817632128580499,\"upperBound\":21.6823678714195,\"significanceLevel\":5}}";
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task DataLengthAndStrataLengthMustBeEqual()
    {
        string body = $$$"""
        {
            "data": [
                9, 10, 11, 18, 22, 25
            ],
            "strata": [
                "m", "m", "m", "f", "f"
            ],
            "stratumSizes": {
                "m": 25,
                "f": 75
            },
            "significanceLevel": 5
        }
        """;
        HttpContent content = Helpers.GetJSONContent(body);
        var response = await _client.PostAsync($"{EstimatorUrl}/stratified", content);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task JSONArrayFormatEndpointIsAlive()
    {
        string body = $$$"""
        [
            {
                "name": "Alice",
                "age": 54,
                "hasPets": false
            },
            {
                "name": "Bob",
                "age": 41,
                "hasPets": true
            },
            {
                "name": "Carol",
                "age": 72,
                "hasPets": true
            }
        ]
        """;
        HttpContent content = Helpers.GetJSONContent(body);
        var response = await _client.PostAsync($"{FormatUrl}/JSONArray", content);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        string actual = await response.Content.ReadAsStringAsync();
        string expected = $$$"""
        {"name":["Alice","Bob","Carol"],"age":[54,41,72],"hasPets":[false,true,true]}
        """;
        Assert.Equal(expected, actual);
    }
}
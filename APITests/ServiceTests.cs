using SamplingAPI.Models.DataTransferModels;
using SamplingAPI.Services;
using SamplingAPI.Services.Interfaces;
using System.Text.Json;

namespace APITests;

public class ServiceTests
{
    private ISampleSizeService sampleSizeService;
    private IFormatService formatService;

    public ServiceTests() 
    {
        sampleSizeService = new SampleSizeService();
        formatService = new FormatService();
    }


    [Fact]
    public void CalculateSampleSizeSRSWithReplacement()
    {
        double ciWidth = 0.028;
        int alpha = 5;
        double worstCasePercentage = 0.7;
        var parameters = new SizeParameters(ciWidth, alpha, true, WorstCasePercentage: worstCasePercentage);

        int actual = sampleSizeService.GetSizeSRS(parameters);
        Assert.Equal(1029, actual);
    }

    [Fact]
    public void CalculateSampleSizeSRSWithoutReplacement()
    {
        double ciWidth = 0.01;
        int alpha = 5;
        double worstCasePercentage = 0.5;
        var parameters = new SizeParameters(ciWidth, alpha, false, 50000, worstCasePercentage);

        int actual = sampleSizeService.GetSizeSRS(parameters);
        Assert.Equal(8057, actual);
    }

    [Fact]
    public void CalculateStratumSizes()
    {
        int sampleSize = 40;
        int[] stratumTotalSizes = [200, 50];
        string[] stratumNames = ["s", "p"];

        var parameters = new StratifiedDistributionParameters(stratumNames, sampleSize, stratumTotalSizes, null, null);

        var actual = sampleSizeService.GetStratifiedDistribution(parameters);
        Dictionary<string, int> expected = new() { { "s", 32 }, { "p", 8 } };
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CalculateStratumSizesWithVariances()
    {
        int sampleSize = 40;
        int[] stratumTotalSizes = [200, 50];
        double[] stratumVariances = [1000000, 81000000];
        string[] stratumNames = ["s", "p"];

        var parameters = new StratifiedDistributionParameters(stratumNames, sampleSize, stratumTotalSizes, stratumVariances, null);

        var actual = sampleSizeService.GetStratifiedDistribution(parameters);
        Dictionary<string, int> expected = new() { { "s", 12 }, { "p", 28 } };
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CalculateStratumSizesWithCosts()
    {
        int sampleSize = 40;
        int[] stratumTotalSizes = [200, 50];
        double[] stratumVariances = [1000000, 81000000];
        double[] stratumCosts = [49, 144];
        string[] stratumNames = ["s", "p"];

        var parameters = new StratifiedDistributionParameters(stratumNames, sampleSize, stratumTotalSizes, stratumVariances, stratumCosts);

        var actual = sampleSizeService.GetStratifiedDistribution(parameters);
        Dictionary<string, int> expected = new() { { "s", 17 }, { "p", 23 } };
        Assert.Equal(expected, actual);
    }

    //[Fact]
    //public void FormatCSV()
    //{
    //    string dataString = """
    //        name,age,married
    //        "Alice",34,true
    //        "Bob",22,true
    //        "Carol",25,false
    //        """;

    //    DelimitedData data = new(dataString, ',', true);

    //    Data expected = formatService.FormatDelimited(data);
    //}
}
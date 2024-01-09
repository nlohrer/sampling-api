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
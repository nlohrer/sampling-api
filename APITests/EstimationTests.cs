using SamplingAPI.MathHelpers;

namespace APITests;

public class EstimationTests
{
    [Theory]
    [InlineData(0.0)]
    [InlineData(9.0, 10.0, 11.0, 18.0, 22.0)]
    [InlineData(-10.0, 10.0, 24.0, -232.0)]
    public void EstimateSRSMeans(params double[] values)
    {
        double actual = Math.Round(MeanFunctions.SRSMean(values), 4);
        double expected = Math.Round(values.Average(), 4);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0.0, 0.0, 0.0)]
    [InlineData(6.5, 9.0, 10.0, 11.0, 18.0, 22.0)]
    public void EstimateSRSVarianceWithReplacement(double exp, params double[] values)
    {
        double mean = MeanFunctions.SRSMean(values);
        double actual = Math.Round(VarianceFunctions.SRSVarianceWithReplacement(values, mean), 4);
        double expected = Math.Round(exp, 4);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0.0, 50, 0.0, 0.0)]
    [InlineData(4.875, 20, 9.0, 10.0, 11.0, 18.0, 22.0)]
    [InlineData(5.85, 50, 9.0, 10.0, 11.0, 18.0, 22.0)]
    public void CalculateSRSVarianceWithoutReplacement(double exp, int populationSize, params double[] values)
    {
        double mean = MeanFunctions.SRSMean(values);
        double actual = Math.Round(VarianceFunctions.SRSVarianceWithoutReplacement(values, mean, populationSize), 4);
        double expected = Math.Round(exp, 4);
        Assert.Equal(expected, actual);
    }
}
using Microsoft.AspNetCore.Routing.Constraints;
using SamplingAPI.Stats;

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
        double actual = Math.Round(VarianceFunctions.SRSVariance(values, mean, true), 4);
        double expected = Math.Round(exp, 4);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0.0, 50, 0.0, 0.0)]
    [InlineData(4.875, 20, 9.0, 10.0, 11.0, 18.0, 22.0)]
    [InlineData(5.85, 50, 9.0, 10.0, 11.0, 18.0, 22.0)]
    public void EstimateSRSVarianceWithoutReplacement(double exp, int populationSize, params double[] values)
    {
        double mean = MeanFunctions.SRSMean(values);
        double actual = Math.Round(VarianceFunctions.SRSVariance(values, mean, false, populationSize), 4);
        double expected = Math.Round(exp, 4);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(14.0, 1, 2, 3)]
    [InlineData(13.0, 1, 2, 4)]
    [InlineData(14.3333, 1, 2, 5)]
    [InlineData(13.3333, 1, 3, 4)]
    [InlineData(14.6667, 1, 3, 5)]
    [InlineData(13.6667, 1, 4, 5)]
    [InlineData(13.6667, 2, 3, 4)]
    [InlineData(15.0, 2, 3, 5)]
    [InlineData(14.0, 2, 4, 5)]
    [InlineData(14.3333, 3, 4, 5)]
    public void EstimateDiffMeans(double exp, params int[] drawn)
    {
        IEnumerable<double> primaryData = [9, 10, 11, 18, 22];
        IEnumerable<double> secondaryData = [11, 11, 11, 21, 21];
        double[] primaryDrawn = primaryData.Where((_, i) => drawn.Contains(i + 1)).ToArray();
        double[] secondaryDrawn = secondaryData.Where((_, i) => drawn.Contains(i + 1)).ToArray();
        double secondaryTotalMean = 15;

        double actual = Math.Round(MeanFunctions.DiffMean(primaryDrawn, secondaryDrawn, secondaryTotalMean), 4);
        double expected = Math.Round(exp, 4);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0.13, 1, 2, 3)]
    [InlineData(0.13, 1, 2, 4)]
    [InlineData(0.31, 1, 2, 5)]
    [InlineData(0.31, 1, 3, 4)]
    [InlineData(0.31, 1, 3, 5)]
    [InlineData(0.58, 1, 4, 5)]
    [InlineData(0.31, 2, 3, 4)]
    [InlineData(0.13, 2, 3, 5)]
    [InlineData(0.53, 2, 4, 5)]
    [InlineData(0.58, 3, 4, 5)]
    public void EstimateDiffVariance(double exp, params int[] drawn)
    {
        IEnumerable<double> primaryData = [9, 10, 11, 18, 22];
        IEnumerable<double> secondaryData = [11, 11, 11, 21, 21];
        double[] primaryDrawn = primaryData.Where((_, i) => drawn.Contains(i + 1)).ToArray();
        double[] secondaryDrawn = secondaryData.Where((_, i) => drawn.Contains(i + 1)).ToArray();
        int populationSize = 5;

        double actual = Math.Round(VarianceFunctions.DiffVariance(primaryDrawn, secondaryDrawn, populationSize), 2);
        double expected = Math.Round(exp, 2);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(13.64, 1, 2, 3)]
    [InlineData(12.91, 1, 2, 4)]
    [InlineData(14.3, 1, 2, 5)]
    [InlineData(13.26, 1, 3, 4)]
    [InlineData(14.65, 1, 3, 5)]
    [InlineData(13.87, 1, 4, 5)]
    [InlineData(13.6, 2, 3, 4)]
    [InlineData(15.0, 2, 3, 5)]
    [InlineData(14.15, 2, 4, 5)]
    [InlineData(14.43, 3, 4, 5)]
    public void EstimateRatioMeans(double exp, params int[] drawn)
    {
        IEnumerable<double> primaryData = [9, 10, 11, 18, 22];
        IEnumerable<double> secondaryData = [11, 11, 11, 21, 21];
        double[] primaryDrawn = primaryData.Where((_, i) => drawn.Contains(i + 1)).ToArray();
        double[] secondaryDrawn = secondaryData.Where((_, i) => drawn.Contains(i + 1)).ToArray();
        double secondaryTotalMean = 15;
        double actual = Math.Round(MeanFunctions.RatioMean(primaryDrawn, secondaryDrawn, secondaryTotalMean), 2);
        double expected = Math.Round(exp, 2);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0.13, 1, 2, 3)]
    [InlineData(0.03, 1, 2, 4)]
    [InlineData(0.42, 1, 2, 5)]
    [InlineData(0.16, 1, 3, 4)]
    [InlineData(0.35, 1, 3, 5)]
    [InlineData(0.67, 1, 4, 5)]
    [InlineData(0.14, 2, 3, 4)]
    [InlineData(0.13, 2, 3, 5)]
    [InlineData(0.55, 2, 4, 5)]
    [InlineData(0.55, 3, 4, 5)]
    public void EstimateRatioVariance(double exp, params int[] drawn)
    {
        IEnumerable<double> primaryData = [9, 10, 11, 18, 22];
        IEnumerable<double> secondaryData = [11, 11, 11, 21, 21];
        double[] primaryDrawn = primaryData.Where((_, i) => drawn.Contains(i + 1)).ToArray();
        double[] secondaryDrawn = secondaryData.Where((_, i) => drawn.Contains(i + 1)).ToArray();
        int populationSize = 5;

        double actual = Math.Round(VarianceFunctions.RatioVariance(primaryDrawn, secondaryDrawn, populationSize), 2);
        double expected = Math.Round(exp, 2);
        Assert.Equal(expected, actual);
    }
}
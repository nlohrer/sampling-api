using Microsoft.AspNetCore.Routing.Constraints;
using SamplingAPI.Services;
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

    [Theory]
    [InlineData(20, 1, 2, 3)]
    [InlineData(20.5, 1, 2, 4)]
    [InlineData(36.5, 1, 2, 5)]
    [InlineData(16, 1, 3, 4)]
    [InlineData(32, 1, 3, 5)]
    [InlineData(32.5, 1, 4, 5)]
    [InlineData(21.5, 2, 3, 4)]
    [InlineData(37.5, 2, 3, 5)]
    [InlineData(38, 2, 4, 5)]
    [InlineData(33.5, 3, 4, 5)]
    public void EstimateHTMeans(double exp, params int[] drawn)
    {
        IEnumerable<double> data = [9, 10, 11, 18, 22];
        IEnumerable<double> inclusionProbabilities = [0.1, 0.05, 0.1, 0.15, 0.05];
        double[] dataDrawn = data.Where((_, i) => drawn.Contains(i + 1)).ToArray();
        double[] drawnProbabilities = inclusionProbabilities.Where((_, i) => drawn.Contains(i + 1)).ToArray();
        int populationSize = 20;

        double actual = Math.Round(MeanFunctions.HTMean(dataDrawn, drawnProbabilities, populationSize), 2);
        double expected = Math.Round(exp, 2);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(25.75, 1, 2, 3)]
    [InlineData(24.25, 1, 2, 4)]
    [InlineData(240.25, 1, 2, 5)]
    [InlineData(1.75, 1, 3, 4)]
    [InlineData(289.75, 1, 3, 5)]
    [InlineData(282.25, 1, 4, 5)]
    [InlineData(18.25, 2, 3, 4)]
    [InlineData(218.25, 2, 3, 5)]
    [InlineData(208.00, 2, 4, 5)]
    [InlineData(264.25, 3, 4, 5)]
    public void EstimateHHVariance(double exp, params int[] drawn)
    {
        IEnumerable<double> data = [9, 10, 11, 18, 22];
        IEnumerable<double> inclusionProbabilities = [0.1, 0.05, 0.1, 0.15, 0.05];
        double[] dataDrawn = data.Where((_, i) => drawn.Contains(i + 1)).ToArray();
        double[] drawnProbabilities = inclusionProbabilities.Where((_, i) => drawn.Contains(i + 1)).ToArray();
        int populationSize = 20;
        double mean = MeanFunctions.HTMean(dataDrawn, drawnProbabilities, populationSize);

        double actual = Math.Round(VarianceFunctions.HHVariance(dataDrawn, drawnProbabilities, mean, populationSize), 2);
        double expected = Math.Round(exp, 2);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(17.38, 1, 2, 4, 5)]
    [InlineData(17.5, 1, 3, 4, 5)]
    [InlineData(17.62, 2, 3, 4, 5)]
    [InlineData(18.5, 1, 2, 4, 6)]
    [InlineData(18.62, 1, 3, 4, 6)]
    [InlineData(18.75, 2, 3, 4, 6)]
    [InlineData(20, 1, 2, 5, 6)]
    [InlineData(20.12, 1, 3, 5, 6)]
    [InlineData(20.25, 2, 3, 5, 6)]
    public void EstimateStratifiedMeans(double exp, params int[] drawn)
    {
        IEnumerable<double> data = [9, 10, 11, 18, 22, 25];
        IEnumerable<string> strata = ["m", "m", "m", "f", "f", "f"];
        double[] dataDrawn = data.Where((_, i) => drawn.Contains(i + 1)).ToArray();
        string[] drawnStrata = strata.Where((_, i) => drawn.Contains(i + 1)).ToArray();
        Dictionary<string, int> stratumSizes = new() {{"m", 25}, {"f", 75}};

        double actual = Math.Round(MeanFunctions.StratifiedMean(dataDrawn, drawnStrata, stratumSizes), 2);
        double expected = Math.Round(exp, 2);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(2.20, 1, 2, 4, 5)]
    [InlineData(2.25, 1, 3, 4, 5)]
    [InlineData(2.20, 2, 3, 4, 5)]
    [InlineData(6.72, 1, 2, 4, 6)]
    [InlineData(6.76, 1, 3, 4, 6)]
    [InlineData(6.72, 2, 3, 4, 6)]
    [InlineData(1.25, 1, 2, 5, 6)]
    [InlineData(1.29, 1, 3, 5, 6)]
    [InlineData(1.25, 2, 3, 5, 6)]
    public void EstimateStratifiedVariance(double exp, params int[] drawn)
    {
        IEnumerable<double> data = [9, 10, 11, 18, 22, 25];
        IEnumerable<string> strata = ["m", "m", "m", "f", "f", "f"];
        double[] dataDrawn = data.Where((_, i) => drawn.Contains(i + 1)).ToArray();
        string[] drawnStrata = strata.Where((_, i) => drawn.Contains(i + 1)).ToArray();
        Dictionary<string, int> stratumSizes = new() {{"m", 25}, {"f", 75}};

        double actual = Math.Round(VarianceFunctions.StratifiedVariance(dataDrawn, drawnStrata, stratumSizes), 2);
        double expected = Math.Round(exp, 2);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void EstimateClusterMeans()
    {
        double[] data = [135, 180, 160, 225];
        int totalClusterCount = 40;
        int clusterCount = 4;
        int populationSize = 2000;

        double actual = MeanFunctions.ClusterMean(data, clusterCount, totalClusterCount, populationSize);
        double expected = 3.5;

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void EstimateClusterVariance()
    {
        double[] data = [135, 180, 160, 225];
        int totalClusterCount = 40;
        int clusterCount = 4;
        int populationSize = 2000;

        double actual = VarianceFunctions.ClusterVariance(data, clusterCount, totalClusterCount, populationSize);
        double expected = 0.1305;

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void EstimateHeterogeneousClusterMeans()
    {
        double[] data = [135, 180, 160, 225];
        int[] clusterSizes = [50, 60, 40, 50];

        double actual = MeanFunctions.HeterogeneousClusterMean(data, clusterSizes);
        double expected = 3.5;

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void EstimateHeterogeneousClusterVariance()
    {
        double[] data = [135, 180, 160, 225];
        int[] clusterSizes = [50, 60, 40, 50];
        double mean = MeanFunctions.HeterogeneousClusterMean(data, clusterSizes);
        int totalClusterCount = 40;
        int clusterCount = 4;
        int populationSize = 2000;


        double actual = VarianceFunctions.HeterogeneousClusterVariance(data, clusterSizes, mean, clusterCount, totalClusterCount, populationSize);
        double expected = 0.162;

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CalculateSampleSizeSRSWithReplacement()
    {
        ISampleSizeService sampleSizeService = new SampleSizeService();
        double ciWidth = 0.028;
        int alpha = 5;
        double worstCasePercentage = 0.7;

        int actual = sampleSizeService.GetSizeSRS(ciWidth, alpha, true, worstCasePercentage: worstCasePercentage);
        Assert.Equal(1029, actual);
    }

    [Fact]
    public void CalculateSampleSizeSRSWithoutReplacement()
    {
        ISampleSizeService sampleSizeService = new SampleSizeService();
        double ciWidth = 0.01;
        int alpha = 5;
        double worstCasePercentage = 0.5;

        int actual = sampleSizeService.GetSizeSRS(ciWidth, alpha, false, 50000, worstCasePercentage);
        Assert.Equal(8057, actual);
    }
}
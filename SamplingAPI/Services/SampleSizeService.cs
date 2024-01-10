using Microsoft.AspNetCore.Routing.Constraints;
using SamplingAPI.Models.DataTransferModels;
using SamplingAPI.Services.Interfaces;
using SamplingAPI.Stats;

namespace SamplingAPI.Services;

/// <summary>
/// Provides methods for calculating sample sizes.
/// </summary>
public class SampleSizeService : ISampleSizeService
{
    /// <summary>
    /// Determines the minimum number of observations to include for a simple random sample.
    /// </summary>
    /// <param name="parameters">A SizeParameters object representing the parameters to determine the actual size.</param>
    /// <returns>The minimum number of observations to include to receive the desired confidence interval.</returns>
    public int GetSizeSRS(SizeParameters parameters)
    {
        double z = GeneralFunctions.GetStandardNormalDistributionQuantile(parameters.Alpha);
        double p = parameters.WorstCasePercentage;
        double e = parameters.E;

        if (parameters.WithReplacement)
        {
            double exactResult = p * (1 - p) *
                Math.Pow(z / e, 2);
            return (int) Math.Ceiling(exactResult);
        } else
        {
            if (parameters.PopulationSize is null) throw new ArgumentNullException();
            double N = parameters.PopulationSize.Value;
            double s = p * (1 - p);
            double exactResult = s /
                (Math.Pow(e / z, 2) + s / N);
            return (int)Math.Ceiling(exactResult);
        }
    }

    /// <summary>
    /// Determines how many observations to include per stratum for a stratified sample.
    /// </summary>
    /// <param name="parameters">A StratifiedDistributionParameters object representing the parameters needed to determine the stratum sizes.</param>
    /// <returns>The number of observations to include for each stratum.</returns>
    public Dictionary<string, int> GetStratifiedDistribution(StratifiedDistributionParameters parameters)
    {
        IEnumerable<string> names;
        if (parameters.StratumNames is null)
        {
            names = Enumerable.Range(1, parameters.StratumTotalSizes.Length).Select(num => $"stratum{num}");
        } else
        {
            names = parameters.StratumNames;
        }
        IEnumerable<double> varianceFactor = parameters.StratumVariances is not null
            ? parameters.StratumVariances.AsEnumerable()
            : Enumerable.Repeat(1.0, parameters.StratumTotalSizes.Length);
        IEnumerable<double> costFactor = parameters.StratumVariances is not null && parameters.StratumCosts is not null
            ? parameters.StratumCosts.AsEnumerable()
            : Enumerable.Repeat(1.0, parameters.StratumTotalSizes.Length);

        IEnumerable<(int N_h, double S_h, double c_h)> zippedData = parameters.StratumTotalSizes.Zip(varianceFactor, costFactor);

        IEnumerable<double> stratumProducts = zippedData.Select(tuple => (tuple.N_h * Math.Sqrt(tuple.S_h)) / Math.Sqrt(tuple.c_h));
        double totalSum = stratumProducts.Sum();

        IEnumerable<int> stratumSizes = stratumProducts.Select(product => (int) Math.Round(parameters.SampleSize * product / totalSum));

        return names.Zip(stratumSizes).ToDictionary();
    }}
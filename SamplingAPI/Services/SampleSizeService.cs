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
}
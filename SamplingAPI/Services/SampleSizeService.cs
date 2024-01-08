using SamplingAPI.Models.DaterTransferModels;
using SamplingAPI.Services.Interfaces;
using SamplingAPI.Stats;
using System.Runtime.InteropServices;

namespace SamplingAPI.Services;

public class SampleSizeService : ISampleSizeService
{
    /// <summary>
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
            double N = parameters.PopulationSize.Value;
            double s = p * (1 - p);
            double exactResult = s /
                (Math.Pow(e / z, 2) + s / N);
            return (int)Math.Ceiling(exactResult);
        }
    }
}
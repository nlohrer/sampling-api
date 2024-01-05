using SamplingAPI.MathHelpers;
using SamplingAPI.Models;

namespace SamplingAPI.Services;

public class EstimationService : IEstimationService
{
    /// <summary>
    /// Estimate the mean of a simple ranodm sample.
    /// </summary>
    /// <param name="srs">A SimpleRandomSample object representing the sample data and additional information required for estimation.</param>
    /// <returns>An estimator for the mean of the variance.</returns>
    public Estimator EstimateSRS(SimpleRandomSample srs)
    {
        double[] data = srs.Data[srs.TargetColumn];
        double mean = MeanFunctions.SRSMean(data);

        double variance;
        if (srs.WithReplacement || srs.PopulationSize is null)
        {
            variance = VarianceFunctions.SRSVarianceWithReplacement(data, mean);
        }
        else
        {
            variance = VarianceFunctions.SRSVarianceWithoutReplacement(data, mean, (int) srs.PopulationSize);
        }
        ConfidenceInterval ci = GeneralFunctions.CalculateConfidenceInterval(mean, variance, srs.SignificanceLevel);
        return new Estimator(mean, variance, ci);
    }

}
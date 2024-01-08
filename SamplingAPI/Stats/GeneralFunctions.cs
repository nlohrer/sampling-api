using SamplingAPI.Models;

namespace SamplingAPI.Stats;

public class GeneralFunctions
{
    /// <summary>
    /// Calculates a confidence interval from the given mean, variance, and significance level, with the used quantiles being based on the standard normal distribution.
    /// </summary>
    /// <exception cref="ArgumentException">If a significance level other than the implemented ones is chosen.</exception>
    public static ConfidenceInterval CalculateConfidenceInterval(double mean, double variance, int significanceLevel)
    {
        double z = GetStandardNormalDistributionQuantile(significanceLevel);
        double width = z * Math.Sqrt(variance);
        return new ConfidenceInterval(mean - width, mean + width, significanceLevel);
    }

    /// <summary>
    /// Get the <paramref name="significanceLevel"/> percentile of the standard normal distribution.
    /// </summary>
    /// <param name="significanceLevel">The desired significance level.</param>
    /// <returns>The <paramref name="significanceLevel"/> percentile of the standard normal distribution.</returns>
    /// <exception cref="NotImplementedException">If the given percentile is not 1, 5, or 10.</exception>
    public static double GetStandardNormalDistributionQuantile(int significanceLevel)
    {
        switch (significanceLevel)
        {
            case 10: 
                return 1.64;
            case 5:
                return 1.96;
            case 1:
                return 2.58;
            default:
                throw new NotImplementedException();
        }
    }
}
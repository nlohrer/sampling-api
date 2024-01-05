using SamplingAPI.Models;

namespace SamplingAPI.MathHelpers;

public class GeneralFunctions
{
    /// <summary>
    /// Calculates a confidence interval from the given mean, variance, and significance level, with the used quantiles being based on the standard normal distribution.
    /// </summary>
    /// <exception cref="ArgumentException">If a significance level other than the implemented ones is chosen.</exception>
    public static ConfidenceInterval CalculateConfidenceInterval(double mean, double variance, int significanceLevel)
    {
        if (significanceLevel == 5)
        {
            double width = 1.96 * Math.Sqrt(variance);
            return new ConfidenceInterval(mean - width, mean + width, significanceLevel);
        }
        throw new ArgumentException("Wrong significance level");
    }
}
using SamplingAPI.Stats;
using System.Runtime.InteropServices;

namespace SamplingAPI.Services;

public class SampleSizeService : ISampleSizeService
{
    /// <summary>
    /// Gets the minimum number of observations to include in a sample, such that there is at least a (1 - <paramref name="alpha"/>)% probability that the difference between the estimated mean and the actual mean is at most (<paramref name="e"/> * 100)% of the actual mean. In other words: the minimum number of observations to include such that the confidence level for the estimated mean has the width <paramref name="e"/> * 2, given significance level <paramref name="alpha"/>.
    /// </summary>
    /// <param name="e">The maximum deviation of the estimated mean from the actual mean. In other words, half the length of the confidence interval of the mean.</param>
    /// <param name="alpha">The significance level for the confidence interval.</param>
    /// <param name="withReplacement">Whether the sample is to be drawn with replacement. If so, you need to specify the <paramref name="populationSize"/> as well.</param>
    /// <param name="populationSize">The total size of the population.</param>
    /// <param name="worstCasePercentage">The worst case for the mean as a proportion of the population. The closer to 0.5, the worse.</param>
    /// <returns>The minimum number of observations to include to receive the desired confidence interval.</returns>
    public int GetSizeSRS(double e, int alpha, bool withReplacement, [Optional] int? populationSize, double worstCasePercentage = 0.5)
    {
        double z = GeneralFunctions.GetStandardNormalDistributionQuantile(alpha);
        double p = worstCasePercentage;

        if (withReplacement)
        {
            double exactResult = p * (1 - p) *
                Math.Pow(z / e, 2);
            return (int) Math.Ceiling(exactResult);
        } else
        {
            if (populationSize is null)
            {
                throw new ArgumentNullException();
            }
            double N = populationSize.Value;
            double s = p * (1 - p);
            double exactResult = s /
                (Math.Pow(e / z, 2) + s / N);
            return (int)Math.Ceiling(exactResult);
        }
    }
}
using System.Runtime.InteropServices;

namespace SamplingAPI.Stats;

public class VarianceFunctions
{
    /// <summary>
    /// Estimates the variance of an estimated mean for a simple random sample. Defaults to drawing with replacement if <paramref name="withReplacement"/> is false but no <paramref name="populationSize"/> is provided.
    /// </summary>
    /// <param name="data">The data used for the mean estimation.</param>
    /// <param name="mean">The estimated mean.</param>
    /// <param name="withReplacement">Whether the sample was drawn with replacement.</param>
    /// <param name="populationSize">The size of the total population. Only necessary if the sample was drawn with replacement.</param>
    /// <returns>The estimated variance of the mean.</returns>
    public static double SRSVariance(double[] data, double mean, bool withReplacement, [OptionalAttribute] int? populationSize)
    {
        double n = data.Length;
        double populationFactor = 1;
        if (!withReplacement)
        {
            if (populationSize is not null)
            {
                double N = (double) populationSize;
                populationFactor = (N - n) / N;
            }
        }

        return 
            populationFactor *
            (1 / (n * (n - 1))) *
            data.Select(y => Math.Pow(y - mean, 2)).Sum();
    }
}
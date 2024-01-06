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

    /// <summary>
    /// Estimates the variance of a mean that was estimated with a difference model.
    /// </summary>
    /// <param name="primaryData">The data used for the mean estimation.</param>
    /// <param name="secondaryData">The auxiliary data that was used for the mean estimation.</param>
    /// <param name="populationSize">The size of the total population.</param>
    /// <returns>The estimated variance of the estimated mean.</returns>
    public static double DiffVariance(double[] primaryData, double[] secondaryData, int populationSize)
    {
        double n = primaryData.Length;
        double N = populationSize;
        double y_bar = primaryData.Average();
        double x_bar = secondaryData.Average();

        double populationFactor = (N - n) / N;
        IEnumerable<(double y, double x)> zippedData = primaryData.Zip(secondaryData);
        double sumOfSquares = zippedData.Select(tuple =>
        {
            double difference = tuple.y - tuple.x - (y_bar - x_bar);
            return Math.Pow(difference, 2);
        }).Sum();

        return 
            populationFactor *
            (1.0 / (n * (n - 1))) *
            sumOfSquares;
    }

    /// <summary>
    /// Estimates the variance of a mean that was estimated with a ratio model.
    /// </summary>
    /// <param name="primaryData">The data used for the mean estimation.</param>
    /// <param name="secondaryData">The auxiliary data that was used for the mean estimation.</param>
    /// <param name="populationSize">The size of the total population.</param>
    /// <returns>The estimated variance of the estimated mean.</returns>
    public static double RatioVariance(double[] primaryData, double[] secondaryData, int populationSize)
    {
        double n = primaryData.Length;
        double N = populationSize;
        double y_bar = primaryData.Average();
        double x_bar = secondaryData.Average();

        double populationFactor = (N - n) / N;
        IEnumerable<(double y, double x)> zippedData = primaryData.Zip(secondaryData);
        double sumOfSquares = zippedData.Select(tuple =>
        {
            double difference = tuple.y - (y_bar / x_bar) * tuple.x;
            return Math.Pow(difference, 2);
        }).Sum();

        return 
            populationFactor *
            (1.0 / (n * (n - 1))) *
            sumOfSquares;
    }
}
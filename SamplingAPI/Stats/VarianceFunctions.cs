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
    /// <param name="populationSize">The total size of the population. Only necessary if the sample was drawn with replacement.</param>
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
    /// <param name="populationSize">The total size of the population.</param>
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
    /// <param name="populationSize">The total size of the population.</param>
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

    /// <summary>
    /// Estimates the variance of a mean with Hansen-Hurwitz estimation.
    /// </summary>
    /// <param name="data">The data used for the mean estimation.</param>
    /// <param name="inclusionProbabilities">The respective inclusion probabilities for the sample data.</param>
    /// <param name="mean">The estimated mean.</param>
    /// <param name="populationSize">The total size of the population.</param>
    /// <returns>The estimated variance of the estimated mean.</returns>
    public static double HHVariance(double[] data, double[] inclusionProbabilities, double mean, int populationSize)
    {
        double n = data.Length;
        double N = populationSize;

        IEnumerable<(double y, double p)> zippedData = data.Zip(inclusionProbabilities.Select(pi => pi / n));
        double sumOfSquares = zippedData.Select(tuple =>
        {
            double difference = tuple.y / (N * tuple.p) - mean;
            return Math.Pow(difference, 2);
        }).Sum();

        return
            (1.0 / (n * (n - 1))) *
            sumOfSquares;
    }

    /// <summary>
    /// Estimates the variance of a mean that was estimated from a stratified sample.
    /// </summary>
    /// <param name="data">The data used for the mean estimation.</param>
    /// <param name="strata">The stratum that each entry of the sample data belongs to respectively.</param>
    /// <param name="stratumSizes">The total population size for each stratum.</param>
    /// <returns>The estimated variance of the estimated mean.</returns>
    public static double StratifiedVariance(double[] data, string[] strata, Dictionary<string, int> stratumSizes)
    {
        IEnumerable<(double y, string stratum)> zippedData = data.Zip(strata);
        double N = stratumSizes.Values.Sum();

        double sum = 0;
        foreach (string stratum in stratumSizes.Keys)
        {
            double N_h = stratumSizes[stratum];
            IEnumerable<(double y, string stratum)> stratumData = zippedData.Where(tuple => tuple.stratum == stratum);
            double n_h = stratumData.Count();

            double factor = Math.Pow(N_h / N, 2) * ((N_h - n_h) / N_h) * (1.0 / n_h);
            double stratumMean = stratumData.Select(tuple => tuple.y).Average();
            double variance = stratumData.Select(tuple =>
            {
                double difference = Math.Pow(tuple.y - stratumMean, 2);
                return difference / (n_h - 1);
            }).Sum();

            sum += factor * variance;
        }
        return sum;
    }

    /// <summary>
    /// Estimates the variance of a mean that was estimated from a cluster sample.
    /// </summary>
    /// <param name="data">The data used for mean estimation.</param>
    /// <param name="clusterCount">The amount of clusters in the sample.</param>
    /// <param name="totalClusterCount">The total amount of clusters in the population.</param>
    /// <param name="populationSize">The total size of the population.</param>
    /// <returns>The estimated mean.</returns>
    public static double ClusterVariance(double[] data, int clusterCount, int totalClusterCount, int populationSize)
    {
        double M = totalClusterCount;
        double m = clusterCount;
        double N = populationSize;
        double clusterTotalMean = data.Average();

        double sumOfSquares = data.Select(clusterTotal => Math.Pow(clusterTotal - clusterTotalMean, 2)).Sum();

        return
            Math.Pow(M / N, 2) *
            ((M - m) / M) *
            (1.0 / (m * (m - 1))) *
            sumOfSquares;
    }
}
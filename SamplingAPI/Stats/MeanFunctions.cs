namespace SamplingAPI.Stats;

/// <summary>
/// Methods for estimating the mean of a population.
/// </summary>
public class MeanFunctions
{
    /// <summary>
    /// Estimates the mean of a population from a simple random sample.
    /// </summary>
    /// <param name="data">The sample data.</param>
    /// <returns>The estimated mean.</returns>
    public static double SRSMean(double[] data)
    {
        return data.Sum() / data.Length;
    }

    /// <summary>
    /// Estimates the mean of a population from a sample based on a difference model.
    /// </summary>
    /// <param name="primaryData">The sample data.</param>
    /// <param name="secondaryData">The auxiliary data used in the model.</param>
    /// <param name="secondaryTotalMean">The mean of the auxiliary data in the entire population.</param>
    /// <returns>The estimated mean.</returns>
    public static double DiffMean(double[] primaryData, double[] secondaryData, double secondaryTotalMean)
    {
        int n = primaryData.Length;
        double difference = (1.0 / n) * primaryData.Zip(secondaryData).Select(tuple => tuple.First - tuple.Second).Sum();
        return
            secondaryTotalMean +
            difference;
    }

    /// <summary>
    /// Estimates the mean of a population from a sample based on a ratio model.
    /// </summary>
    /// <param name="primaryData">The sample data.</param>
    /// <param name="secondaryData">The auxiliary data used in the model.</param>
    /// <param name="secondaryTotalMean">The mean of the auxiliary data in the entire population.</param>
    /// <returns>The estimated mean.</returns>
    public static double RatioMean(double[] primaryData, double[] secondaryData, double secondaryTotalMean)
    {
        double primaryMean = primaryData.Average();
        double secondaryMean = secondaryData.Average();
        double ratio = primaryMean / secondaryMean;
        return ratio * secondaryTotalMean;
    }

    /// <summary>
    /// Horvitz-Thompson estimator for the mean of a population.
    /// </summary>
    /// <param name="data">The sample data.</param>
    /// <param name="inclusionProbabilities">The respective inclusion probabilities for the sample data.</param>
    /// <param name="populationSize">The total size of the population.</param>
    /// <returns>The estimated mean.</returns>
    public static double HTMean(double[] data, double[] inclusionProbabilities, int populationSize)
    {
        double N = populationSize;
        IEnumerable<(double y, double pi)> zippedData = data.Zip(inclusionProbabilities);
        double sum = zippedData.Select(tuple => 1.0 * tuple.y / tuple.pi).Sum();
        return (1.0 / N) * sum;
    }

    /// <summary>
    /// Estimates the mean of a population from a stratified sample.
    /// </summary>
    /// <param name="data">The sample data.</param>
    /// <param name="strata">The stratum that each entry of the sample data belongs to respectively.</param>
    /// <param name="stratumSizes">The total population size for each stratum.</param>
    /// <returns>The estimated mean.</returns>
    public static double StratifiedMean(double[] data, string[] strata, Dictionary<string, int> stratumSizes)
    {
        IEnumerable<(double y, string stratum)> zippedData = data.Zip(strata);
        double N = stratumSizes.Values.Sum();

        double sum = 0;
        foreach (string stratum in stratumSizes.Keys)
        {
            sum += (1.0 * stratumSizes[stratum] / N) * zippedData
                .Where(tuple => tuple.stratum == stratum)
                .Select(tuple => tuple.y)
                .Average();
        }
        return sum;
    }

    /// <summary>
    /// Estimates the mean of a population based on a cluster sampling strategy when the clusters are roughly equal in size.
    /// </summary>
    /// <param name="data">The sample data.</param>
    /// <param name="clusterCount">The amount of clusters in the sample.</param>
    /// <param name="totalClusterCount">The total amount of clusters in the population.</param>
    /// <param name="populationSize">The total size of the population.</param>
    /// <returns>The estimated mean.</returns>
    public static double ClusterMean(double[] data, int clusterCount, int totalClusterCount, int populationSize)
    {
        double M = totalClusterCount;
        double m = clusterCount;
        double N = populationSize;

        return (M / N) * data.Sum() / m;
    }

    /// <summary>
    /// Estimates the mean of a population based on a cluster sampling strategy when the clusters differ in size.
    /// </summary>
    /// <param name="data">The sample data.</param>
    /// <param name="clusterSizes">The size of each cluster.</param>
    /// <returns>The estimated mean.</returns>
    public static double HeterogeneousClusterMean(double[] data, int[] clusterSizes)
    {
        return data.Sum() / clusterSizes.Sum();
    }
}
namespace SamplingAPI.Stats;

public class MeanFunctions
{
    /// <summary>
    /// Estimates the mean of a simple random sample.
    /// </summary>
    /// <param name="data">The sample data.</param>
    /// <returns>The estimated mean.</returns>
    public static double SRSMean(double[] data)
    {
        return data.Sum() / data.Length;
    }

    /// <summary>
    /// Estimates the mean of a sample based on a difference model.
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
    /// Estimates the mean of a sample based on a ratio model.
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
    /// Horvitz-Thompson estimator for the mean of a sample.
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
}
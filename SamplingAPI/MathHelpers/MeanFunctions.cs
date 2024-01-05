namespace SamplingAPI.MathHelpers;

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
}
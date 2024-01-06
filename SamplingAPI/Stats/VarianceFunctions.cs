namespace SamplingAPI.Stats;

public class VarianceFunctions
{
    /// <summary>
    /// Estimates the variance of an estimated mean for a simple random sample with replacement.
    /// </summary>
    public static double SRSVarianceWithReplacement(double[] data, double mean)
    {
        double n = data.Length;
        return 
            (1 / (n * (n - 1))) *
            data.Select(y => Math.Pow(y - mean, 2)).Sum();
    }

    /// <summary>
    /// Estimates the variance of an estimated mean for a simple random simple without replacement.
    /// </summary>
    public static double SRSVarianceWithoutReplacement(double[] data, double mean, int populationSize)
    {
        double n = data.Length;
        double N = populationSize;
        return
            ((N - n) / N) *
            (1 / (n * (n - 1))) *
            data.Select(y => Math.Pow(y - mean, 2)).Sum();
        ;
    }
}
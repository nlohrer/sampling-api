namespace SamplingAPI.Services;

public interface ISamplingService
{
    /// <summary>
    /// Take a sample of size <paramref name="n"/> from the given <paramref name="data"/>.
    /// </summary>
    /// <param name="data">The original data.</param>
    /// <param name="n">The size of the sample.</param>
    /// <param name="withReplacement">Whether the sample should be drawn with replacement.</param>
    /// <returns>A sample drawn from the original <paramref name="data"/>.</returns>
    Dictionary<string, List<double>> TakeSimpleRandomSample(Dictionary<string, double[]> data, int n, bool withReplacement);
}
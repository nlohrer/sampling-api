using System.Text.Json;

namespace SamplingAPI.Services;

public interface ISamplingService
{
    /// <summary>
    /// Take a sample of size <paramref name="n"/> from the given <paramref name="data"/>.
    /// </summary>
    /// <param name="data">The original data.</param>
    /// <param name="n">The size of the sample.</param>
    /// <param name="withReplacement">Whether the sample should be drawn with replacement.</param>
    /// <param name="removeMissing">Whether rows with null values should not be sampled.</param>
    /// <returns>A sample drawn from the original <paramref name="data"/>.</returns>
    Dictionary<string, List<JsonElement>> TakeSimpleRandomSample(Dictionary<string, JsonElement[]> data, int n, bool withReplacement, bool removeMissing);

    /// <summary>
    /// Take a systematic sample from the given <paramref name="data"/>.
    /// </summary>
    /// <param name="data">The original data.</param>
    /// <param name="interval">The interval for systematic sampling - given k = interval, every kth element will be drawn into the sample.</param>
    /// <param name="firstIndex">The first index for sampling.</param>
    /// <returns>A systematic sample drawn from the original <paramref name="data"/>.</returns>
    Dictionary<string, List<JsonElement>> TakeSystematicSample(Dictionary<string, JsonElement[]> data, int interval, int firstIndex = 0);
}
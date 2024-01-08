
using System.Text.Json;

namespace SamplingAPI.Services;

public class SamplingService : ISamplingService
{
    Random random = new Random();

    /// <summary>
    /// Take a sample of size <paramref name="n"/> from the given <paramref name="data"/>.
    /// </summary>
    /// <param name="data">The original data.</param>
    /// <param name="n">The size of the sample.</param>
    /// <param name="withReplacement">Whether the sample should be drawn with replacement.</param>
    /// <returns>A sample drawn from the original <paramref name="data"/>.</returns>
    public Dictionary<string, List<JsonElement>> TakeSimpleRandomSample(Dictionary<string, JsonElement[]> data, int n, bool withReplacement)
    {
        int length = data.Values.ElementAt(0).Length;
        if (withReplacement && n > length)
        {
            n = length;
        }

        Dictionary<string, List<JsonElement>> sample = new();
        foreach (string key in data.Keys)
        {
            sample[key] = [];
        }
        IEnumerable<int> randomNumbers;
        if (withReplacement)
        {
            randomNumbers = Enumerable
                .Range(0, length)
                .OrderBy(i => random.Next())
                .Take(n);
        } else
        {
            randomNumbers = new List<int>();
            for (int _ = 0; _ < n; _++)
            {
                ((List<int>)randomNumbers).Add(random.Next(0, length));
            }
        }

        foreach (int rand in randomNumbers)
        {
            foreach(string key in data.Keys)
            {
                sample[key].Add(data[key].ElementAt(rand));
            }
        }

        return sample;
    }
}
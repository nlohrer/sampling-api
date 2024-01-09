using SamplingAPI.Models.DataTransferModels;
using SamplingAPI.Services.Interfaces;
using System.Text.Json;

namespace SamplingAPI.Services;

/// <summary>
/// Provides methods for taking samples from data.
/// </summary>
public class SamplingService : ISamplingService
{
    Random random = new Random();

    /// <summary>
    /// Take a sample of size <paramref name="n"/> from the given <paramref name="data"/>.
    /// </summary>
    /// <param name="data">The original data.</param>
    /// <param name="n">The size of the sample.</param>
    /// <param name="withReplacement">Whether the sample should be drawn with replacement.</param>
    /// <param name="removeMissing">Whether rows with null values should not be sampled.</param>
    /// <returns>A sample drawn from the original <paramref name="data"/>.</returns>
    public Dictionary<string, List<JsonElement>> TakeSimpleRandomSample(Data data, int n, bool withReplacement, bool removeMissing)
    {

        int length = data.Values.ElementAt(0).Count;
        if (removeMissing)
        {
            length -= RemoveRowsWithMissingValues(data);
        }

        if (!withReplacement && n > length)
        {
            n = length;
        }

        Dictionary<string, List<JsonElement>> sample = new();
        foreach (string key in data.Keys)
        {
            sample[key] = [];
        }
        IEnumerable<int> randomNumbers;
        if (!withReplacement)
        {
            randomNumbers = Enumerable
                .Range(0, length)
                .OrderBy(i => random.Next())
                .Take(n);
        }
        else
        {
            randomNumbers = new List<int>();
            for (int _ = 0; _ < n; _++)
            {
                ((List<int>)randomNumbers).Add(random.Next(0, length));
            }
        }

        foreach (int rand in randomNumbers)
        {
            foreach (string key in data.Keys)
            {
                sample[key].Add(data[key].ElementAt(rand));
            }
        }

        return sample;
    }

    /// <summary>
    /// Take a systematic sample from the given <paramref name="data"/>.
    /// </summary>
    /// <param name="data">The original data.</param>
    /// <param name="interval">The interval for systematic sampling - given k = interval, every kth element will be drawn into the sample.</param>
    /// <param name="firstIndex">The first index for sampling.</param>
    /// <returns>A systematic sample drawn from the original <paramref name="data"/>.</returns>
    public Dictionary<string, List<JsonElement>> TakeSystematicSample(Data data, int interval, int firstIndex = 0)
    {
        int length = data.Values.ElementAt(0).Count;

        Dictionary<string, List<JsonElement>> sample = new();
        foreach (string key in data.Keys)
        {
            sample[key] = [];
        }

        for (int i = firstIndex; i < length; i += interval)
        {
            foreach (string key in data.Keys)
            {
                List<JsonElement> column = data[key];
                sample[key].Add(column[i]);
            }
        }

        return sample;
    }

    /// <summary>
    /// Removes all rows from the data that contain missing values and returns the number of rows removed.
    /// </summary>
    /// <returns>The number of removed rows.</returns>
    private static int RemoveRowsWithMissingValues(Data data)
    {
        HashSet<int> missingIndices = [];
        foreach (List<JsonElement> column in data.Values)
        {
            for (int i = 0; i < column.Count; i++)
            {
                if (column[i].ValueKind == JsonValueKind.Null)
                {
                    missingIndices.Add(i);
                }
            }
        }

        foreach (string key in data.Keys)
        {
            data[key] = data[key].Where((_, i) => !missingIndices.Contains(i)).ToList();
        }

        return missingIndices.Count;
    }
}
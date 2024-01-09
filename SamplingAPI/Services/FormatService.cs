using SamplingAPI.Models.DataTransferModels;
using SamplingAPI.Services.Interfaces;
using System.Text.Json;

namespace SamplingAPI.Services;

/// <summary>
/// Provides methods for formatting data into the format used by this API.
/// </summary>
public class FormatService : IFormatService
{
    /// <summary>
    /// Formats delimiter-seperated values into the format used by this API.
    /// </summary>
    /// <param name="data">A DelimitedData object representing the string containing the data as well as additional information for formatting the data.</param>
    /// <returns>A Dictionary with data in the format used by this API.</returns>
    public Data FormatDelimited(DelimitedData data)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Formats values that were given as an array of JSON objects into the format used by this API.
    /// </summary>
    /// <param name="data">An array of JSON objects.</param>
    /// <returns>A Dictionary with data in the format used by this API.</returns>
    public Data FormatJsonObjectArray(Dictionary<string, JsonElement>[] data)
    {
        var firstEntry = data.First();

        var columnNames = firstEntry.Keys;
        Data formattedData = new Data();
        foreach (string name in columnNames)
        {
            formattedData[name] = [];
        }

        foreach (var row in data)
        {
            foreach (string name in columnNames)
            {
                formattedData[name].Add(row[name]);
            }
        }

        return formattedData;
    }
}
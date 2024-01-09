using SamplingAPI.Models.DataTransferModels;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace SamplingAPI.Services.Interfaces;

public interface IFormatService
{
    /// <summary>
    /// Formats delimiter-seperated values into the format used by this API.
    /// </summary>
    /// <param name="data">A DelimitedData object representing the string containing the data as well as additional information for formatting the data.</param>
    /// <returns>A Dictionary with data in the format used by this API.</returns>
    Data FormatDelimited(DelimitedData data);

    /// <summary>
    /// Formats values that were given as an array of JSON objects into the format used by this API.
    /// </summary>
    /// <param name="data">An array of JSON objects.</param>
    /// <returns>A Dictionary with data in the format used by this API.</returns>
    Data FormatJsonObjectArray(Dictionary<string, JsonElement>[] data);
}
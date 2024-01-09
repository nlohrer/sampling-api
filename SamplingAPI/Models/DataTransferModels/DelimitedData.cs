namespace SamplingAPI.Models.DataTransferModels;

/// <summary>
/// Represents delimiter-seperated data.
/// </summary>
/// <param name="Data">The string representing the data.</param>
/// <param name="Delimiter">The delimiter used to seperate the data values.</param>
/// <param name="TreatFirstRowAsColumnNames">Whether the first row should be used to infer the column names.</param>
public record DelimitedData(string Data, char Delimiter, bool TreatFirstRowAsColumnNames = true);
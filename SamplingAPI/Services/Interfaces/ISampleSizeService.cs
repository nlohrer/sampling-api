using SamplingAPI.Models.DataTransferModels;

namespace SamplingAPI.Services.Interfaces;

/// <summary>
/// Provides methods for calculating sample sizes.
/// </summary>
public interface ISampleSizeService
{
    /// <summary>
    /// Determines the minimum number of observations to include for a simple random sample.
    /// </summary>
    /// <param name="parameters">A SizeParameters object representing the parameters to determine the actual size.</param>
    /// <returns>The minimum number of observations to include to receive the desired confidence interval.</returns>
    int GetSizeSRS(SizeParameters parameters);
}
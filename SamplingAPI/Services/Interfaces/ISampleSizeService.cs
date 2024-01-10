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

    /// <summary>
    /// Determines how many observations to include per stratum for a stratified sample.
    /// </summary>
    /// <param name="parameters">A StratifiedDistributionParameters object representing the parameters needed to determine the stratum sizes.</param>
    /// <returns>The number of observations to include for each stratum.</returns>
    Dictionary<string, int> GetStratifiedDistribution(StratifiedDistributionParameters parameters);
}
namespace SamplingAPI.Models.DataTransferModels;

/// <summary>
/// Represents the parameters needed to determine how many observations of each stratum to include for a stratified sample, given a fixed sample size.
/// </summary>
/// <param name="StratumNames">The name of each stratum.</param>
/// <param name="SampleSize">The size of the sample.</param>
/// <param name="StratumTotalSizes">The total size of each stratum.</param>
/// <param name="StratumVariances">The variance for each stratum.</param>
/// <param name="StratumCosts">The cost for including an observation per stratum.</param>
public record StratifiedDistributionParameters(string[]? StratumNames, int SampleSize, int[] StratumTotalSizes, double[]? StratumVariances, double[]? StratumCosts);
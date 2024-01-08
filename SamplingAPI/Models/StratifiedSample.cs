namespace SamplingAPI.Models;

/// <summary>
/// Represents a stratified sample, including metadata for estimation.
/// </summary>
/// <param name="Data">The sample itself.</param>
/// <param name="Strata">The stratum that each entry of the sample data belongs to respectively.</param>
/// <param name="StratumSizes">The total population size for each stratum.</param>
/// <param name="SignificanceLevel">The desired significance level (as percentage) for the returned confidence interval.</param>
public record StratifiedSample(double[] Data, string[] Strata, Dictionary<string, int> StratumSizes, int SignificanceLevel);
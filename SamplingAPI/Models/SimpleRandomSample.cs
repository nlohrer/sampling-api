namespace SamplingAPI.Models;

/// <summary>
/// Represents a simple random sample, including metadata for estimation.
/// </summary>
/// <param name="TargetColumn">The column whose mean should be estimated.</param>
/// <param name="WithReplacement">Whether the sample was drawn with replacement.</param>
/// <param name="PopulationSize">The total size of the population.</param>
/// <param name="Data">The sample itself.</param>
/// <param name="SignificanceLevel">The desired significance level (as percentage) for the returned confidence interval.</param>
public record SimpleRandomSample(string TargetColumn, bool WithReplacement, int? PopulationSize, Dictionary<string, double[]> Data, int SignificanceLevel);

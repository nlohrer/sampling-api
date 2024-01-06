namespace SamplingAPI.Models;

/// <summary>
/// Represents a sample for design-based estimation, including metadata for estimation.
/// </summary>
/// <param name="TargetColumn">The name of the variable whose mean should be estimated.</param>
/// <param name="InclusionProbabilityColumn">The name of the variable that specifies the estimation probabilities for the variable used for the estimation.</param>
/// <param name="PopulationSize">The total size of the population.</param>
/// <param name="Data">The sample itself.</param>
/// <param name="SignificanceLevel">The desired significance level (as percentage) for the returned confidence interval.</param>
public record DesignSample(string TargetColumn, string InclusionProbabilityColumn, int PopulationSize, Dictionary<string, double[]> Data, int SignificanceLevel);
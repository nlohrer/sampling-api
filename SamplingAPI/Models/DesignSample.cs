namespace SamplingAPI.Models;

/// <summary>
/// Represents a sample for design-based estimation, including metadata for estimation.
/// </summary>
/// <param name="Data">The sample itself.</param>
/// <param name="InclusionProbabilities">The inclusion probabilities probabilities for the data used for the estimation.</param>
/// <param name="PopulationSize">The total size of the population.</param>
/// <param name="SignificanceLevel">The desired significance level (as percentage) for the returned confidence interval.</param>
public record DesignSample(double[] Data, double[] InclusionProbabilities, int PopulationSize, int SignificanceLevel);
namespace SamplingAPI.Models;

/// <summary>
/// Represents a simple random sample, including metadata for estimation.
/// </summary>
/// <param name="Data">The sample itself.</param>
/// <param name="WithReplacement">Whether the sample was drawn with replacement.</param>
/// <param name="PopulationSize">The total size of the population.</param>
/// <param name="SignificanceLevel">The desired significance level (as percentage) for the returned confidence interval.</param>
public record SimpleRandomSample(double[] Data, bool WithReplacement, int? PopulationSize, int SignificanceLevel);

namespace SamplingAPI.Models;

/// <summary>
/// Represents a sample for cluster-based estimation, including metadata for estimation.
/// </summary>
/// <param name="Data">The sample itself.</param>
/// <param name="ClusterSizes">The size of each cluster.</param>
/// <param name="PopulationSize">The total size of the population.</param>
/// <param name="ClusterCount">The amount of clusters in the sample.</param>
/// <param name="TotalClusterCount">The total amount of clusters in the population.</param>
/// <param name="SignificanceLevel">The desired significance level (as percentage) for the returned confidence interval.</param>
public record ClusterSample(double[] Data, int[]? ClusterSizes, int PopulationSize, int ClusterCount, int TotalClusterCount, int SignificanceLevel);
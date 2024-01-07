namespace SamplingAPI.Models;

/// <summary>
/// Represents a sample for cluster-based estimation, including metadata for estimation.
/// </summary>
/// <param name="TargetColumn">The name of the variable whose mean should be estimated.</param>
/// <param name="ClusterSizes">The size of each cluster.</param>
/// <param name="PopulationSize">The total size of the population.</param>
/// <param name="ClusterCount">The amount of clusters in the sample.</param>
/// <param name="TotalClusterCount">The total amount of clusters in the population.</param>
/// <param name="Data">The sample itself.</param>
/// <param name="SignificanceLevel">The desired significance level (as percentage) for the returned confidence interval.</param>
public record ClusterSample(string TargetColumn, int[]? ClusterSizes, int PopulationSize, int ClusterCount, int TotalClusterCount, Dictionary<string, double[]> Data, int SignificanceLevel);
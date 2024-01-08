using System.ComponentModel.DataAnnotations;

namespace SamplingAPI.Models.SampleModels;

/// <summary>
/// Represents a sample for cluster-based estimation, including metadata for estimation.
/// </summary>
/// <param name="Data">The sample itself.</param>
/// <param name="ClusterSizes">The size of each cluster.</param>
/// <param name="PopulationSize">The total size of the population.</param>
/// <param name="ClusterCount">The amount of clusters in the sample.</param>
/// <param name="TotalClusterCount">The total amount of clusters in the population.</param>
/// <param name="SignificanceLevel">The desired significance level (as percentage) for the returned confidence interval.</param>
public record ClusterSample(double[] Data, int[]? ClusterSizes, int PopulationSize, int ClusterCount, int TotalClusterCount, int SignificanceLevel) : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (ClusterSizes is not null && Data.Length != ClusterSizes.Length)
        {
            yield return new ValidationResult(
                $"You need to specify exactly one cluster size for each data entry.",
                new[] { nameof(Data) });
        }
    }
}
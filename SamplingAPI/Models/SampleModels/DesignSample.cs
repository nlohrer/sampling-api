using System.ComponentModel.DataAnnotations;

namespace SamplingAPI.Models.SampleModels;

/// <summary>
/// Represents a sample for design-based estimation, including metadata for estimation.
/// </summary>
/// <param name="Data">The sample itself.</param>
/// <param name="InclusionProbabilities">The inclusion probabilities probabilities for the data used for the estimation.</param>
/// <param name="PopulationSize">The total size of the population.</param>
/// <param name="SignificanceLevel">The desired significance level (as percentage) for the returned confidence interval.</param>
public record DesignSample(double[] Data, double[] InclusionProbabilities, int PopulationSize, int SignificanceLevel) : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Data.Length != InclusionProbabilities.Length)
        {
            yield return new ValidationResult(
                $"You need to specify exactly one inclusion probability for each data entry.",
                new[] { nameof(Data) });
        }

        if (InclusionProbabilities.Sum() > 1)
        {
            yield return new ValidationResult(
                $"The sum of all inclusion probabilities must not exceed 1",
                new[] { nameof(InclusionProbabilities) });
        }
    }
}

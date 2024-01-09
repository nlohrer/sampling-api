using System.ComponentModel.DataAnnotations;

namespace SamplingAPI.Models.SampleModels;

/// <summary>
/// Represents a sample for model-based estimation, including metadata for estimation.
/// </summary>
/// <param name="Data">The sample itself.</param>
/// <param name="AuxiliaryData">The auxiliary data used for the design.</param>
/// <param name="AuxiliaryMean">The mean in the total population for the auxiliary variable.</param>
/// <param name="PopulationSize">The total size of the population.</param>
/// <param name="SignificanceLevel">The desired significance level (as percentage) for the returned confidence interval.</param>
public record ModelSample(double[] Data, double[] AuxiliaryData, double AuxiliaryMean, int PopulationSize, int SignificanceLevel) : IValidatableObject
{
    /// <summary>
    /// Determines whether the data is valid.
    /// </summary>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Data.Length != AuxiliaryData.Length)
        {
            yield return new ValidationResult(
                $"You need to specify exactly one auxiliary data point for each main data entry.",
                new[] { nameof(Data) });
        }
    }
}
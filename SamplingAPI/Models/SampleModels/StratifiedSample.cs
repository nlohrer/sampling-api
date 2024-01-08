using System.ComponentModel.DataAnnotations;

namespace SamplingAPI.Models.SampleModels;

/// <summary>
/// Represents a stratified sample, including metadata for estimation.
/// </summary>
/// <param name="Data">The sample itself.</param>
/// <param name="Strata">The stratum that each entry of the sample data belongs to respectively.</param>
/// <param name="StratumSizes">The total population size for each stratum.</param>
/// <param name="SignificanceLevel">The desired significance level (as percentage) for the returned confidence interval.</param>
public record StratifiedSample(double[] Data, string[] Strata, Dictionary<string, int> StratumSizes, int SignificanceLevel) : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Data.Length != Strata.Length)
        {
            yield return new ValidationResult(
                $"You need to specify exactly one stratum for each data entry.",
                new[] { nameof(Data) });
        }


        if (Strata.GroupBy(stratum => stratum).Select(stratum => stratum.Count()).Any(count => count < 2))
        {
            yield return new ValidationResult(
                $"Each stratum must occur at least twice in the sample.",
                new[] { nameof(Strata) });
        }

        if (Strata.Any(stratum => !StratumSizes.ContainsKey(stratum)))
        {
            yield return new ValidationResult(
                $"All strata must have a corresponding key in stratumSizes",
                new[] { nameof(Strata) });
        }
    }
}
using System.ComponentModel.DataAnnotations;

namespace SamplingAPI.Models.DataTransferModels;

/// <summary>
/// Represents the parameters needed to determine how many observations of each stratum to include for a stratified sample, given a fixed sample size.
/// </summary>
/// <param name="StratumNames">The name of each stratum.</param>
/// <param name="SampleSize">The size of the sample.</param>
/// <param name="StratumTotalSizes">The total size of each stratum.</param>
/// <param name="StratumVariances">The variance for each stratum.</param>
/// <param name="StratumCosts">The cost for including an observation per stratum.</param>
public record StratifiedDistributionParameters(string[]? StratumNames, int SampleSize, int[] StratumTotalSizes, double[]? StratumVariances, double[]? StratumCosts) : IValidatableObject
{
    /// <summary>
    /// Determines whether the data is valid.
    /// </summary>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        int?[] lengths = [NullableLength(StratumNames), NullableLength(StratumVariances), NullableLength(StratumCosts)];
        foreach (int? length in lengths)
        {
            if (length is null) continue;
            if (!(StratumTotalSizes.Length == length))
            {
                yield return new ValidationResult(
                    $"All provided arrays must have the same length",
                    new[] { nameof(StratumTotalSizes) });
            }
        }
    }

    private int? NullableLength(Array? array)
    {
        return array?.Length;
    }
}

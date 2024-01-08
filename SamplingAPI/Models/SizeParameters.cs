using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace SamplingAPI.Models;

/// <summary>
/// Represents the parameters needed to estimate the minimum number of observations to include in a sample, such that there is at least a (1 - <paramref name="Alpha"/>)% probability that the difference between the estimated mean and the actual mean is at most (<paramref name="e"/> * 100)% of the actual mean. In other words: the minimum number of observations to include such that the confidence level for the estimated mean has the width <paramref name="E"/> * 2, given significance level <paramref name="Alpha"/>.
/// </summary>
/// <param name="E">The maximum deviation of the estimated mean from the actual mean. In other words, half the length of the confidence interval of the mean.</param>
/// <param name="Alpha">The significance level for the confidence interval.</param>
/// <param name="WithReplacement">Whether the sample is to be drawn with replacement. If so, you need to specify the <paramref name="PopulationSize"/> as well.</param>
/// <param name="PopulationSize">The total size of the population.</param>
/// <param name="WorstCasePercentage">The worst case for the mean as a proportion of the population. The closer to 0.5, the worse.</param>
public record SizeParameters(double E, int Alpha, bool WithReplacement, [Optional] int? PopulationSize, double WorstCasePercentage = 0.5) : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!WithReplacement && PopulationSize is null)
        {
            yield return new ValidationResult(
                $"You need to specify the population size when drawing without replacement.",
                new[] { nameof(PopulationSize) });
        }

        if (WorstCasePercentage < 0 || 1 < WorstCasePercentage)
        {
            yield return new ValidationResult(
                $"WorstCasePercentage must be between 0 and 1.",
                new[] { nameof(WorstCasePercentage) });
        }
    }
}
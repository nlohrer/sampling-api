using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace SamplingAPI.Models.DataTransferModels;

/// <summary>
/// Represents generic tabular data.
/// </summary>
public class Data : Dictionary<string, List<JsonElement>>, IValidatableObject
{
    /// <summary>
    /// Determines whether the data is valid.
    /// </summary>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Count > 0)
        {
            var lengths = Values.Select(column => column.Count);
            int firstLength = lengths.First();

            if (lengths.Any(length => length != firstLength))
            {
                yield return new ValidationResult(
                    $"All columns must be of same length.",
                    new[] { nameof(Data) });
            }
        }
    }
}
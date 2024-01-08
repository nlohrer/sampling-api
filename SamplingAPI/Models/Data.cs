using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace SamplingAPI.Models;

/// <summary>
/// Represents generic tabular data.
/// </summary>
public class Data : Dictionary<string, JsonElement[]>, IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (this.Count > 0)
        {
            var lengths = this.Values.Select(column => column.Length);
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
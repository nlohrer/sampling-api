using SamplingAPI.Models.DaterTransferModels;
namespace SamplingAPI.Services.Interfaces;

public interface ISampleSizeService
{
    /// <summary>
    /// Determines the minimum number of observations to include for a simple random sample.
    /// <param name="parameters">A SizeParameters object representing the parameters to determine the actual size.</param>
    /// <returns>The minimum number of observations to include to receive the desired confidence interval.</returns>
    int GetSizeSRS(SizeParameters parameters);
}
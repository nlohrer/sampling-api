using SamplingAPI.Models;

namespace SamplingAPI.Services;

public interface IEstimationService
{
    /// <summary>
    /// Estimate the mean of a simple ranodm sample.
    /// </summary>
    /// <param name="srs">A SimpleRandomSample object representing the sample data and additional information required for estimation.</param>
    /// <returns>An estimator for the mean of the variance.</returns>
    Estimator EstimateSRS(SimpleRandomSample srs);
}
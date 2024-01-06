using SamplingAPI.Models;

namespace SamplingAPI.Services;

public interface IEstimationService
{
    /// <summary>
    /// Estimate the mean of a simple ranodm sample.
    /// </summary>
    /// <param name="srs">A SimpleRandomSample object representing the sample data and additional information required for estimation.</param>
    /// <returns>An estimator for the mean of the entire population.</returns>
    Estimator EstimateSRS(SimpleRandomSample srs);

    /// <summary>
    /// Estimate the mean of a sample with the help of a model.
    /// </summary>
    /// <param name="srs">A SimpleRandom object representing the sample data and additional information required for estimation.</param>
    /// <param name="modelType">The type of model that should be used for the estimation.</param>
    /// <returns>An estimator for the mean of the entire population.</returns>
    Estimator EstimateModel(ModelSample sample, ModelType modelType);

    /// <summary>
    /// Estimate the mean of a sample based on a design.
    /// </summary>
    /// <param name="sample">A DesignSample object representing the sample data and additional iniformation required for estimation.</param>
    /// <returns>An estimator for the mean of the entire population.</returns>
    Estimator EstimateDesign(DesignSample sample);
}

public enum ModelType {
    diff,
    ratio
}
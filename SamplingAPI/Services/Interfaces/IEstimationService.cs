using SamplingAPI.Models.DaterTransferModels;
using SamplingAPI.Models.SampleModels;

namespace SamplingAPI.Services.Interfaces;

public interface IEstimationService
{
    /// <summary>
    /// Estimate the mean of a population from a simple random sample.
    /// </summary>
    /// <param name="srs">A SimpleRandomSample object representing the sample data and additional information required for estimation.</param>
    /// <returns>An estimator for the mean of the entire population.</returns>
    Estimator EstimateSRS(SimpleRandomSample srs);

    /// <summary>
    /// Estimate the mean of a population from a sample based on a  model.
    /// </summary>
    /// <param name="srs">A SimpleRandom object representing the sample data and additional information required for estimation.</param>
    /// <param name="modelType">The type of model that should be used for the estimation.</param>
    /// <returns>An estimator for the mean of the entire population.</returns>
    Estimator EstimateModel(ModelSample sample, ModelType modelType);

    /// <summary>
    /// Estimate the mean of a population from a sample that was drawn based on a design.
    /// </summary>
    /// <param name="sample">A DesignSample object representing the sample data and additional iniformation required for estimation.</param>
    /// <returns>An estimator for the mean of the entire population.</returns>
    Estimator EstimateDesign(DesignSample sample);

    /// <summary>
    /// Estimates the mean of a population from a stratified sample.
    /// </summary>
    /// <param name="sample">A StratifiedSample object representing the sample data and additional iniformation required for estimation.</param>
    /// <returns>An estimator for the mean of the entire population.</returns>
    Estimator EstimateStratified(StratifiedSample sample);

    /// <summary>
    /// Estimates the mean of a population from a cluster sample.
    /// </summary>
    /// <param name="sample">A ClusterSample object representing the sample data and additional iniformation required for estimation.</param>
    /// <param name="equalSizes">Whether the clusters are (roughly) equal in size.</param>
    /// <returns>An estimator for the mean of the entire population.</returns>
    Estimator EstimateCluster(ClusterSample sample, bool equalSizes);
}

/// <summary>
/// Represents a type of model used for an estimate.
/// </summary>
public enum ModelType
{
    /// <summary>
    /// For difference-based estimation.
    /// </summary>
    diff,
    /// <summary>
    /// For ratio-based estimation.
    /// </summary>
    ratio
}
using SamplingAPI.Stats;
using SamplingAPI.Services.Interfaces;
using SamplingAPI.Models.SampleModels;
using SamplingAPI.Models.DaterTransferModels;

namespace SamplingAPI.Services;

public class EstimationService : IEstimationService
{
    /// <summary>
    /// Estimate the mean of a population from a simple random sample.
    /// </summary>
    /// <param name="srs">A SimpleRandomSample object representing the sample data and additional information required for estimation.</param>
    /// <returns>An estimator for the mean of the total population.</returns>
    public Estimator EstimateSRS(SimpleRandomSample srs)
    {
        double mean = MeanFunctions.SRSMean(srs.Data);

        double variance;
        if (srs.WithReplacement || srs.PopulationSize is null)
        {
            variance = VarianceFunctions.SRSVariance(srs.Data, mean, true);
        }
        else
        {
            variance = VarianceFunctions.SRSVariance(srs.Data, mean, false, (int) srs.PopulationSize);
        }

        ConfidenceInterval ci = GeneralFunctions.CalculateConfidenceInterval(mean, variance, srs.SignificanceLevel);
        return new Estimator(mean, variance, ci);
    }

    /// <summary>
    /// Estimate the mean of a population from a sample based on a  model.
    /// </summary>
    /// <param name="sample">A ModelSample object representing the sample data and additional information required for estimation.</param>
    /// <param name="modelType">The type of model that should be used for the estimation.</param>
    /// <returns>An estimator for the mean of the total population.</returns>
    public Estimator EstimateModel(ModelSample sample, ModelType modelType)
    {
        double mean, variance;
        if (modelType == ModelType.diff)
        {
            mean = MeanFunctions.DiffMean(sample.Data, sample.AuxiliaryData, sample.AuxiliaryMean);
            variance = VarianceFunctions.DiffVariance(sample.Data, sample.AuxiliaryData, sample.PopulationSize);
        } else if (modelType == ModelType.ratio)
        {
            mean = MeanFunctions.RatioMean(sample.Data, sample.AuxiliaryData, sample.AuxiliaryMean);
            variance = VarianceFunctions.RatioVariance(sample.Data, sample.AuxiliaryData, sample.PopulationSize);
        } else
        {
            throw new ArgumentException("Please provide a correct model for estimation.");
        }

        ConfidenceInterval ci = GeneralFunctions.CalculateConfidenceInterval(mean, variance, sample.SignificanceLevel);
        return new Estimator(mean, variance, ci);
    }

    /// <summary>
    /// Estimate the mean of a population from a sample that was drawn based on a design.
    /// </summary>
    /// <param name="sample">A DesignSample object representing the sample data and additional iniformation required for estimation.</param>
    /// <returns>An estimator for the mean of the entire population.</returns>
    public Estimator EstimateDesign(DesignSample sample)
    {
        double mean = MeanFunctions.HTMean(sample.Data, sample.InclusionProbabilities, sample.PopulationSize);
        double variance = VarianceFunctions.HHVariance(sample.Data, sample.InclusionProbabilities, mean, sample.PopulationSize);
        ConfidenceInterval ci = GeneralFunctions.CalculateConfidenceInterval(mean, variance, sample.SignificanceLevel);
        return new Estimator(mean, variance, ci);
    }

    /// <summary>
    /// Estimates the mean of a population from a stratified sample.
    /// </summary>
    /// <param name="sample">A StratifiedSample object representing the sample data and additional iniformation required for estimation.</param>
    /// <returns>An estimator for the mean of the entire population.</returns>
    public Estimator EstimateStratified(StratifiedSample sample)
    {
        double mean = MeanFunctions.StratifiedMean(sample.Data, sample.Strata, sample.StratumSizes);
        double variance = VarianceFunctions.StratifiedVariance(sample.Data, sample.Strata, sample.StratumSizes);
        ConfidenceInterval ci = GeneralFunctions.CalculateConfidenceInterval(mean, variance, sample.SignificanceLevel);
        return new Estimator(mean, variance, ci);
    }

    /// <summary>
    /// Estimates the mean of a population from a cluster sample.
    /// </summary>
    /// <param name="sample">A ClusterSample object representing the sample data and additional iniformation required for estimation.</param>
    /// <param name="equalSizes">Whether the clusters are (roughly) equal in size.</param>
    /// <returns>An estimator for the mean of the entire population.</returns>
    public Estimator EstimateCluster(ClusterSample sample, bool equalSizes)
    {
        double mean, variance;

        if (equalSizes)
        {
            mean = MeanFunctions.ClusterMean(sample.Data, sample.ClusterCount, sample.TotalClusterCount, sample.PopulationSize);
            variance = VarianceFunctions.ClusterVariance(sample.Data, sample.ClusterCount, sample.TotalClusterCount, sample.PopulationSize);
        } else
        {
            if (sample.ClusterSizes is null)
            {
                throw new ArgumentNullException(nameof(sample.ClusterSizes));
            }
            mean = MeanFunctions.HeterogeneousClusterMean(sample.Data, sample.ClusterSizes);
            variance = VarianceFunctions.HeterogeneousClusterVariance(sample.Data, sample.ClusterSizes, mean, sample.ClusterCount, sample.TotalClusterCount, sample.PopulationSize);
        }

        ConfidenceInterval ci = GeneralFunctions.CalculateConfidenceInterval(mean, variance, sample.SignificanceLevel);
        return new Estimator(mean, variance, ci);
    }
}
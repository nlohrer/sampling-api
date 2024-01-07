using SamplingAPI.Stats;
using SamplingAPI.Models;

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
        double[] data = srs.Data[srs.TargetColumn];
        double mean = MeanFunctions.SRSMean(data);

        double variance;
        if (srs.WithReplacement || srs.PopulationSize is null)
        {
            variance = VarianceFunctions.SRSVariance(data, mean, true);
        }
        else
        {
            variance = VarianceFunctions.SRSVariance(data, mean, false, (int) srs.PopulationSize);
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
        double[] primaryData = sample.Data[sample.TargetColumn];
        double[] secondaryData = sample.Data[sample.AuxiliaryColumn];

        double mean, variance;
        if (modelType == ModelType.diff)
        {
            mean = MeanFunctions.DiffMean(primaryData, secondaryData, sample.AuxiliaryMean);
            variance = VarianceFunctions.DiffVariance(primaryData, secondaryData, sample.PopulationSize);
        } else if (modelType == ModelType.ratio)
        {
            mean = MeanFunctions.RatioMean(primaryData, secondaryData, sample.AuxiliaryMean);
            variance = VarianceFunctions.RatioVariance(primaryData, secondaryData, sample.PopulationSize);
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
        double[] data = sample.Data[sample.TargetColumn];
        double[] inclusionProbabilities = sample.Data[sample.InclusionProbabilityColumn];

        double mean = MeanFunctions.HTMean(data, inclusionProbabilities, sample.PopulationSize);
        double variance = VarianceFunctions.HHVariance(data, inclusionProbabilities, mean, sample.PopulationSize);
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
        double[] data = sample.Data[sample.TargetColumn];

        double mean = MeanFunctions.StratifiedMean(data, sample.Strata, sample.StratumSizes);
        double variance = VarianceFunctions.StratifiedVariance(data, sample.Strata, sample.StratumSizes);
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
        double[] data = sample.Data[sample.TargetColumn];
        double mean, variance;

        if (equalSizes)
        {
            mean = MeanFunctions.ClusterMean(data, sample.ClusterCount, sample.TotalClusterCount, sample.PopulationSize);
            variance = VarianceFunctions.ClusterVariance(data, sample.ClusterCount, sample.TotalClusterCount, sample.PopulationSize);
        } else
        {
            if (sample.ClusterSizes is null)
            {
                throw new ArgumentNullException(nameof(sample.ClusterSizes));
            }
            mean = MeanFunctions.HeterogeneousClusterMean(data, sample.ClusterSizes);
            variance = VarianceFunctions.HeterogeneousClusterVariance(data, sample.ClusterSizes, mean, sample.ClusterCount, sample.TotalClusterCount, sample.PopulationSize);
        }

        ConfidenceInterval ci = GeneralFunctions.CalculateConfidenceInterval(mean, variance, sample.SignificanceLevel);
        return new Estimator(mean, variance, ci);
    }
}
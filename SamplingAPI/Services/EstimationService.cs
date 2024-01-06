using SamplingAPI.Stats;
using SamplingAPI.Models;

namespace SamplingAPI.Services;

public class EstimationService : IEstimationService
{
    /// <summary>
    /// Estimate the mean of a simple ranodm sample.
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
    /// Estimate the mean of a sample with the help of a model.
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

}
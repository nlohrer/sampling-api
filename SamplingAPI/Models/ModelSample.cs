namespace SamplingAPI.Models;

/// <summary>
/// Represents a sample for model-based estimation, including metadata for estimation.
/// </summary>
/// <param name="TargetColumn">The name of the variable whose mean should be estimated.</param>
/// <param name="AuxiliaryColumn">The name of the variable to be used as auxiliary information</param>
/// <param name="AuxiliaryMean">The mean in the total population for the auxiliary variable.</param>
/// <param name="PopulationSize">The total size of the population.</param>
/// <param name="Data">The sample itself.</param>
/// <param name="SignificanceLevel">The desired significance level (as percentage) for the returned confidence interval.</param>
public record ModelSample(string TargetColumn, string AuxiliaryColumn, double AuxiliaryMean, int PopulationSize, Dictionary<string, double[]> Data, int SignificanceLevel);
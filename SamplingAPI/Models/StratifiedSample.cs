﻿namespace SamplingAPI.Models;

/// <summary>
/// Represents a stratified sample, including metadata for estimation.
/// </summary>
/// <param name="TargetColumn">The name of the variable whose mean should be estimated.</param>
/// <param name="Strata">The stratum that each entry of the sample data belongs to respectively.</param>
/// <param name="StratumSizes">The total population size for each stratum.</param>
/// <param name="Data">The sample itself.</param>
/// <param name="SignificanceLevel">The desired significance level (as percentage) for the returned confidence interval.</param>
public record StratifiedSample(string TargetColumn, string[] Strata, Dictionary<string, int> StratumSizes, Dictionary<string, double[]> Data, int SignificanceLevel);
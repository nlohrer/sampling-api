﻿using Microsoft.AspNetCore.Mvc;
using SamplingAPI.Models.DataTransferModels;
using SamplingAPI.Services.Interfaces;

namespace SamplingAPI.Controllers;

/// <summary>
/// Determine sample size.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SampleSizeController : ControllerBase
{
    private ISampleSizeService _sizeService;

    /// <summary>
    /// Determine sample size.
    /// </summary>
    public SampleSizeController(ISampleSizeService sizeService)
    {
        _sizeService = sizeService;
    }

    /// <summary>
    /// Determine the minimum number of observations to put into a simple random sample in order to get the desired confidence interval.
    /// </summary>
    /// <param name="parameters">Represents the parameters needed to determine the minimum number of observations to include in a sample, such that there is at least a (1 - alpha)% probability that the difference between the estimated mean and the actual mean is at most (e * 100)% of the actual mean.
    /// <br></br>
    /// In other words: the minimum number of observations to include such that the confidence level for the estimated mean has the width e * 2, given significance level alpha.</param>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /api/SampleSize/srs
    ///     
    ///     {
    ///         "e": 0.02,
    ///         "alpha": 5,
    ///         "withReplacement": false,
    ///         "populationSize": 10000,
    ///         "worstCasePercentage": 0.35
    ///     }
    /// </remarks>
    /// <returns>The minimum number of observations to put into a single random sample for the desired confidence interval.</returns>
    /// <response code="200">If the minimum observation number could be determined.</response>
    /// <response code="400">If the provided data could not be validated.</response>
    [HttpPost("srs")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<int> SRSSize(SizeParameters parameters)
    {
        int count = _sizeService.GetSizeSRS(parameters);
        return Ok(count);
    }

    /// <summary>
    /// Determines how many observations to include per stratum for a stratified sample. If you don't specify a vector of variances, the distribution will be proportional. If you specify a vector of (estimated) variances, the distribution will be variance-optimal. If you specify a vector of (estimated) costs as well, it will be cost-optimal.
    /// </summary>
    /// <param name="parameters">Represents the parameters needed to determine observation count for each stratum.</param>
    /// <returns>The number of observations to include for each stratum.</returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /api/SampleSize/stratified
    ///     
    ///     {
    ///         "stratumNames": [
    ///             "b", "m", "s"
    ///         ],
    ///         "sampleSize": 50,
    ///         "stratumTotalSizes": [
    ///             40, 100, 220
    ///         ],
    ///         "stratumVariances": [
    ///             250, 120, 50
    ///         ],
    ///         "stratumCosts": [
    ///             130, 80, 60
    ///         ]
    ///     }
    /// </remarks>
    /// <response code="200">If the minimum observation number could be determined.</response>
    /// <response code="400">If the provided data could not be validated.</response>
    [HttpPost("stratified/distribution")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Dictionary<string, int>> StratifiedSizes(StratifiedDistributionParameters parameters)
    {
        var counts = _sizeService.GetStratifiedDistribution(parameters);
        return Ok(counts);
    }
}
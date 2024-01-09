using Microsoft.AspNetCore.Mvc;
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
    /// <param name="parameters">Represents the parameters needed to estimate the minimum number of observations to include in a sample, such that there is at least a (1 - alpha)% probability that the difference between the estimated mean and the actual mean is at most (e * 100)% of the actual mean.
    /// <br></br>
    /// In other words: the minimum number of observations to include such that the confidence level for the estimated mean has the width e * 2, given significance level alpha.</param>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /api/SampleSize
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
}
using Microsoft.AspNetCore.Mvc;
using SamplingAPI.Services;

namespace SamplingAPI.Controllers;

/// <summary>
/// Take sample from data.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SampleController : ControllerBase
{
    private ISamplingService _samplingService;

    /// <summary>
    /// Take samples from data.
    /// </summary>
    public SampleController(ISamplingService samplingService)
    {
        _samplingService = samplingService;
    }

    /// <summary>
    /// Take a simple random sample of size <paramref name="n"/> from the provided <paramref name="data"/>.
    /// </summary>
    /// <param name="data">The data the sample should be taken from.</param>
    /// <param name="n">The size of the sample.</param>
    /// <param name="withReplacement">Whether the sample should be drawn with replacement.</param>
    /// <returns>The drawn sample.</returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /api/sample/srs?n=3&amp;withReplacement=false
    ///     {
    ///         "age": [
    ///             18, 24, 38, 48, 52
    ///         ],
    ///         "height": [
    ///             165, 182, 169, 190, 175
    ///         ]
    ///     }
    /// </remarks>
    [HttpPost("srs")]
    public async Task<ActionResult<Dictionary<string, List<double>>>> SampleSRS([FromBody] Dictionary<string, double[]> data, [FromQuery] int n, [FromQuery] bool withReplacement = true)
    {
        Dictionary<string, List<double>> sample = _samplingService.TakeSimpleRandomSample(data, n, withReplacement);

        return Ok(sample);
    }
}
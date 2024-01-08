using Microsoft.AspNetCore.Mvc;
using SamplingAPI.Models.DaterTransferModels;
using SamplingAPI.Services.Interfaces;
using System.Text.Json;

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
    /// <param name="removeMissing">Whether rows with missing data (that is, rows containing null values) should be removed.</param>
    /// <returns>The drawn sample.</returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /api/sample/srs?n=3&amp;withReplacement=false&amp;removeMissing=false
    ///
    ///     {
    ///         "age": [
    ///             18, 24, 38, 48, 52
    ///         ],
    ///         "height": [
    ///             165, 182, 169, 190, 175
    ///         ]
    ///     }
    /// </remarks>
    /// <response code="200">If the data could be sampled successfully.</response>
    /// <response code="400">If the provided data could not be validated.</response>
    [HttpPost("srs")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Dictionary<string, List<JsonElement>>>> SampleSRS([FromBody] Data data, [FromQuery] int n, [FromQuery] bool withReplacement = true, [FromQuery] bool removeMissing = false)
    {
        Dictionary<string, List<JsonElement>> sample = _samplingService.TakeSimpleRandomSample(data, n, withReplacement, removeMissing);

        return Ok(sample);
    }

    /// <summary>
    /// Take a systematic sample from the provided <paramref name="data"/>.
    /// </summary>
    /// <param name="data">The data the sample should be taken from.</param>
    /// <param name="interval">The interval for systematic sampling. For example, given interval = 5, every fifth element is sampled.</param>
    /// <param name="firstIndex">The index of the first element to be sampled.</param>
    /// <returns>The drawn sample.</returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /api/sample/systematic?interval=3
    ///     
    ///     {
    ///         "age": [
    ///             19, 23, 39, 83, 54, 63, 34
    ///         ],
    ///         "name": [
    ///             "Alice", "Bob", "Carol", "Dave", "Erin", "Frank", "Grace"
    ///         ]
    ///     }
    /// </remarks>
    /// <response code="200">If the data could be sampled successfully.</response>
    /// <response code="400">If the provided data could not be validated.</response>
    [HttpPost("systematic")]
    public async Task<ActionResult<Dictionary<string, List<JsonElement>>>> SystematicSample([FromBody] Data data, [FromQuery] int interval, [FromQuery] int firstIndex = 0)
    {
        if (interval < 1)
        {
            ModelState.AddModelError(nameof(interval), "interval must be at least 1");
        }
        if (firstIndex >= interval)
        {
            ModelState.AddModelError(nameof(firstIndex), "firstIndex must be less than interval.");
        }
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Dictionary<string, List<JsonElement>> sample = _samplingService.TakeSystematicSample(data, interval, firstIndex);
        return Ok(sample);
    }
}
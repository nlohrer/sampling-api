using Microsoft.AspNetCore.Mvc;
using SamplingAPI.Models.DataTransferModels;
using SamplingAPI.Services.Interfaces;
using System.Text.Json;

namespace SamplingAPI.Controllers;

/// <summary>
/// Format data.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class FormatController : ControllerBase
{
    private IFormatService _formatService;

    /// <summary>
    /// Format data.
    /// </summary>
    /// <param name="formatService"></param>
    public FormatController(IFormatService formatService)
    {
        _formatService = formatService;
    }

    /// <summary>
    /// Formats tabular data in the form of an array of JSON objects into the format used by this API.
    /// </summary>
    /// <param name="data">An array of JSON objects representing tabular data.</param>
    /// <returns>The formatted data.</returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     [
    ///         {
    ///             "name": "Alice",
    ///             "age": 54,
    ///             "hasPets": "false"
    ///         },
    ///         {
    ///             "name": "Bob",
    ///             "age": 41,
    ///             "hasPets": "true"
    ///         }
    ///     ]
    /// </remarks>
    [HttpPost("JSONArray")]
    public ActionResult<Data> FormatJSONArray([FromBody] Dictionary<string, JsonElement>[] data)
    {
        Data result = _formatService.FormatJsonObjectArray(data);
        return Ok(result);
    }
}
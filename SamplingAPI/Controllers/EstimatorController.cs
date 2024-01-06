using Microsoft.AspNetCore.Mvc;
using SamplingAPI.Models;
using SamplingAPI.Services;

namespace SamplingAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EstimatorController : ControllerBase
{
    private IEstimationService EstimationService;
    public EstimatorController(IEstimationService estimationService)
    {
        EstimationService = estimationService;
    }

    [HttpPost("srs")]
    public async Task<ActionResult<Estimator>> EstimateSRS(SimpleRandomSample srs)
    {
        Estimator result = EstimationService.EstimateSRS(srs);
        return Ok(result);
    }

    /// <summary>
    /// Use a model-based estimation for a population mean based on a sample.
    /// </summary>
    /// <param name="sample">The sample data.</param>
    /// <param name="modelType">The type of the model used for estimation.</param>
    /// <returns>An estimator for the mean of a population.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     {
    ///         "targetColumn": "age",
    ///         "auxiliaryColumn": "height",
    ///         "auxiliaryMean": 15,
    ///         "populationSize": 5,
    ///         "data": {
    ///             "age": [
    ///                 9, 10, 11
    ///             ],
    ///             "height": [
    ///                 11, 11, 11
    ///             ]
    ///         },
    ///         "significanceLevel": 5
    ///     }
    /// </remarks>
    /// <response code="200">If the estimation could be performed successfully</response>
    /// <response code="400">If either the model type or the sample model was not valid.</response>
    [HttpPost("model")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Estimator>> EstimateModelBased(ModelSample sample, [FromQuery] ModelType modelType)
    {
        Estimator result = EstimationService.EstimateModel(sample, modelType);
        return Ok(result);
    }
}

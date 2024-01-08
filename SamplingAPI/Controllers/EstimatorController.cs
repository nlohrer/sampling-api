using Microsoft.AspNetCore.Mvc;
using SamplingAPI.Models;
using SamplingAPI.Services;

namespace SamplingAPI.Controllers;

/// <summary>
/// Estimate the mean of a sample.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class EstimatorController : ControllerBase
{
    private IEstimationService EstimationService;

    /// <summary>
    /// Estimate the mean of a sample.
    /// </summary>
    /// <param name="estimationService"></param>
    public EstimatorController(IEstimationService estimationService)
    {
        EstimationService = estimationService;
    }

    /// <summary>
    /// Estimate the mean for a population based on a simple random sample.
    /// </summary>
    /// <param name="srs">The sample data.</param>
    /// <returns>The estimator for the mean of a population.</returns>
    /// <remarks>
    /// Sample request:
    ///     
    ///     POST /api/estimator/srs
    ///     
    ///     {
    ///         "data": [
    ///             23, 83, 53, 34
    ///         ],
    ///         "withReplacement": false,
    ///         "populationSize": 25,
    ///         "significanceLevel": 5
    ///     }
    /// </remarks>
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
    ///     POST /api/estimator/model?modelType=diff
    ///     
    ///     {
    ///         "data":  [
    ///             9, 10, 11
    ///         ],
    ///         "auxiliaryData": [
    ///             11, 11, 11
    ///         ],
    ///         "auxiliaryMean": 15,
    ///         "populationSize": 5,
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


    /// <summary>
    /// Use a design-based estimation for a population mean based on a sample. In order to avoid second-order inclusion probabilities a Hansen-Hurwitz estimator is always used to estimate the variance of the mean estimator.
    /// </summary>
    /// <param name="sample">The sample data.</param>
    /// <returns>An estimator for the mean of a population.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/estimator/design
    ///     
    ///     {
    ///         "data": [
    ///             4, 9, 24
    ///         ],
    ///         "inclusionProbabilities": [
    ///             0.05, 0.1, 0.125
    ///         ],
    ///         "populationSize": 20,
    ///         "significanceLevel": 5
    ///     }
    /// </remarks>
    /// <response code="200">If the estimation could be performed successfully</response>
    /// <response code="400">If the sample model was not valid.</response>
    [HttpPost("design")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Estimator>> EstimateDesignBased(DesignSample sample)
    {
        Estimator result = EstimationService.EstimateDesign(sample);
        return Ok(result);
    }

    /// <summary>
    /// Estimate a population mean based on a stratified sample.
    /// </summary>
    /// <param name="sample">The sample data.</param>
    /// <returns>An estimator for the mean of a population.</returns>
    /// <remarks>
    /// Sample request:
    ///     
    ///     POST /api/estimator/stratified
    ///     
    ///     {
    ///         "Data": [
    ///             9, 10, 11, 18, 22, 25
    ///         ],
    ///         "strata": [
    ///             "m", "m", "m", "f", "f", "f"
    ///         ],
    ///         "stratumSizes": {
    ///             "m": 25,
    ///             "f": 75
    ///         },
    ///         "significanceLevel": 5
    ///     }
    /// </remarks>
    /// <response code="200">If the estimation could be performed successfully</response>
    /// <response code="400">If the sample model was not valid.</response>
    [HttpPost("stratified")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Estimator>> EstimateStratified(StratifiedSample sample)
    {
        Estimator result = EstimationService.EstimateStratified(sample);
        return Ok(result);
    }

    /// <summary>
    /// Estimate a population mean based on a cluster sample.
    /// </summary>
    /// <param name="sample">The sample data.</param>
    /// <param name="equalSizes">Whether the clusters are (roughly) equal in size.</param>
    /// <returns>An estimator for the mean of a population.</returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /api/estimator/stratified?equalSizes=false
    ///     
    ///     {
    ///         "Data": [
    ///             140, 200, 80
    ///         ],
    ///         "clusterSizes": [
    ///             40, 60, 20
    ///         ],
    ///         "populationSize": 2000,
    ///         "clusterCount": 3,
    ///         "totalClusterCount": 100,
    ///         "significanceLevel": 5
    ///     }
    /// </remarks>
    /// <response code="200">If the estimation could be performed successfully</response>
    /// <response code="400">If the sample model was not valid.</response>
    [HttpPost("cluster")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Estimator>> EstimateCluster(ClusterSample sample, [FromQuery] bool equalSizes=true)
    {
        Estimator result = EstimationService.EstimateCluster(sample, equalSizes);
        return Ok(result);
    }
}

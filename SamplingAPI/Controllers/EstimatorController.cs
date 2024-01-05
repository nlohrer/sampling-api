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
}

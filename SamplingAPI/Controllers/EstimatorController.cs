using Microsoft.AspNetCore.Mvc;

namespace SamplingAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EstimatorController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<string>> HelloWorld()
    {
        return Ok("Hello, world!");
    }
}

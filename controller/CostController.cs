using Microsoft.AspNetCore.Mvc;
using garage_managemet_backend_api.Services;
using garage_managemet_backend_api.Models;

namespace garage_managemet_backend_api.controller;

[ApiController]
[Route("api/[controller]")]
public class CostController : ControllerBase
{
    private readonly CostCalculatorService _calculatorService;

    public CostController(CostCalculatorService calculatorService)
    {
        _calculatorService = calculatorService;
    }

    [HttpGet("test")]
    public async Task<IActionResult> Calculate()
    {
        return Ok("Cost Controller is working!");
    }

    [HttpPost("calculate")]
    public async Task<IActionResult> Calculate([FromBody] CostRequest request)
    {
        var result = await _calculatorService.CalculateAsync(request);
        return Ok(result);
    }
}


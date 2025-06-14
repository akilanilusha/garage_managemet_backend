using System;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace garage_managemet_backend_api.controller;

[ApiController]
[Route("api/[controller]")]
public class SayHelloController :ControllerBase
{
    [HttpGet]
    public IActionResult SayHello()
    {
        return Ok("Say Hello");
    }
}

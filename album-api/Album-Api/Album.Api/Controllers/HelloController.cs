using System;
using Microsoft.AspNetCore.Mvc;
using Album.Api.Models;
using Album.Api.Services;
using Microsoft.Extensions.Logging;

namespace Album.Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class HelloController : ControllerBase
  {
    private readonly ILogger<HelloController> _logger;

    private readonly GreetingService _greetingService;

    // Logger included as param for GreetingService, it does work, but when attempting to run Test, it requires logger again from the TestUnit.
    // public HelloController(ILogger<HelloController> logger, ILogger<GreetingService> greetingServiceLogger)
    // {
    //   _logger = logger;
    //   _greetingService = new GreetingService(greetingServiceLogger);
    // }
    
    public HelloController(ILogger<HelloController> logger)
    {
      _logger = logger;
      _greetingService = new GreetingService();
    }


    // GET: api/<ValuesController>
    [HttpGet]
    public ActionResult Get([FromQuery] string name)
    { 
      Hello validatedInput = new Hello() { Name = name };
      _logger.LogInformation("Client requested HelloController at {DateTime}",DateTime.UtcNow.ToLongTimeString());
      return Ok(_greetingService.Greeting(validatedInput.Name));
    }
  }
}
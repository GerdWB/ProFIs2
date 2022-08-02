namespace WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using RestTest.Model;

[ApiController]
[Route("[controller]")]
public class PFController : ControllerBase
{
    private static readonly string[] Summaries =
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<PFController> _logger;

    public PFController(ILogger<PFController> logger) => _logger = logger;

    [HttpPost]
    public async Task<ActionResult<ProFis2Data>> CreateTodoItem(ProFis2Data todoItemDTO) => Ok(todoItemDTO);
}
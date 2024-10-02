using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class ConsumeMessageController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Kafka consumer is running. Check the console for consumed messages.");
    }
}

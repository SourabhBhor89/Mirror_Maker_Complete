using Microsoft.AspNetCore.Mvc;
using MicroservicesSolution.ServiceA.Services;

[ApiController]
[Route("api")]
public class MessageController : ControllerBase
{
    private readonly ProducerService _producerService;
        
    public MessageController(ProducerService producerService)
    {
        _producerService = producerService;
        
    }

    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok("Service is running");
    }



    [HttpPost("sendToA")]
public async Task<IActionResult> SendMessage([FromBody] MessageDto messageDto)
{
    if (messageDto == null || string.IsNullOrEmpty(messageDto.Message))
    {
        return BadRequest("Message cannot be empty");
    }

   messageDto.Dtime = DateTime.Now;
    messageDto.Origin = "ServiceA";

    try
    {
        await _producerService.ProduceAsync("service-a-topic", messageDto);
        return Ok("Message sent to service-a-topic with origin");
    }
    catch
    {
        return StatusCode(500, "Error sending message");
    }
}
    
}

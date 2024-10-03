using Microsoft.AspNetCore.Mvc;
using MicroservicesSolution.ServiceA.Services;

[ApiController]
[Route("api")]
public class MessageController : ControllerBase
{
    private readonly KafkaProducerService _kafkaProducerService;

    public MessageController(KafkaProducerService kafkaProducerService)
    {
        _kafkaProducerService = kafkaProducerService;
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

   
    messageDto.Origin = "ServiceA";

    try
    {
        await _kafkaProducerService.ProduceAsync("service-a-topic", messageDto);
        return Ok("Message sent to service-a-topic with origin");
    }
    catch
    {
        return StatusCode(500, "Error sending message");
    }
}
    
}

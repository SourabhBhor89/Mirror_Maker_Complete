using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
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

    [HttpPost]
    public async Task<IActionResult> SendMessage([FromBody] MessageDto messageDto)
    {
        if (messageDto == null || string.IsNullOrEmpty(messageDto.Message))
        {
            return BadRequest("Message cannot be empty");
        }

        try
        {
            await _kafkaProducerService.ProduceAsync("service-a-topic", messageDto.Message);
            return Ok("Message sent to Source");
        }
        catch
        {
            return StatusCode(500, "Error sending message");
        }
    }
}

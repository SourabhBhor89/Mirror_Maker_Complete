using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api")]
public class ConsumeMessageController : ControllerBase
{
    private readonly KafkaProducerService _kafkaProducerService;

    public ConsumeMessageController(KafkaProducerService kafkaProducerService)
    {
        _kafkaProducerService = kafkaProducerService;
    }

    [HttpPost("sendToB")]
    public async Task<IActionResult> SendMessage([FromBody] MessageDto messageDto)
    {
        if (messageDto == null || string.IsNullOrEmpty(messageDto.Message))
        {
            return BadRequest("Message cannot be empty");
        }


        messageDto.Dtime = DateTime.Now;
        messageDto.Origin = "ServiceB";

        try
        {
            await _kafkaProducerService.PublishMessageAsync("service-b-topic", messageDto);
            return Ok("Message sent to service-b-topic with origin");
        }
        catch
        {
            return StatusCode(500, "Error sending message");
        }
    }
}

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api")]
public class ConsumeMessageController : ControllerBase
{
    private readonly ServiceB _serviceB;

    public ConsumeMessageController(ServiceB serviceB)
    {
        _serviceB = serviceB;
    }

    [HttpPost("sendToB")]
    public async Task<IActionResult> SendMessage([FromBody] MessageDto messageDto)
    {
        if (messageDto == null || string.IsNullOrEmpty(messageDto.Message))
        {
            return BadRequest("Message cannot be empty");
        }

        messageDto.Origin = "ServiceB";

        try
        {
            await _serviceB.PublishMessageAsync("service-b-topic", messageDto);
            return Ok("Message sent to service-b-topic with origin");
        }
        catch
        {
            return StatusCode(500, "Error sending message");
        }
    }
}

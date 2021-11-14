using MicroLog.Core.Extensions;

namespace MicroLog.Collector.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Collector : ControllerBase
{
    private ILogPublisher _LogPublisher { get; set; }

    public Collector(ILogPublisher logPublisher)
    {
        _LogPublisher = logPublisher;
    }

    [AllowAnonymous]
    [HttpPost("insert")]
    public async Task<IActionResult> Insert(LogEvent logEvent)
    {
        try
        {
            if (logEvent.IsValid(out string error))
            {
                await _LogPublisher.PublishAsync(logEvent);
                return Ok();
            }
            else
            {
                return BadRequest(error);
            }
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
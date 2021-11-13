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
            await _LogPublisher.PublishAsync(logEvent);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
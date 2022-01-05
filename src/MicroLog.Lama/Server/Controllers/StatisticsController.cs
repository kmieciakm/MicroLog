namespace MircoLog.Lama.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatisticsController : ControllerBase
{
    private readonly ILogger<StatisticsController> _logger;
    private ILogStatsProvider _StatsProvider { get; }

    public StatisticsController(ILogger<StatisticsController> logger, ILogStatsProvider statsProvider)
    {
        _logger = logger;
        _StatsProvider = statsProvider;
    }

    [HttpGet("daily")]
    public ActionResult<LogsStatistics> GetDailyStatistics()
    {
        return _StatsProvider.GetDailyStatistics();
    }

    [HttpGet("total")]
    public ActionResult<LogsStatistics> GetTotalStatistics()
    {
        return _StatsProvider.GetTotalStatistics();
    }
}
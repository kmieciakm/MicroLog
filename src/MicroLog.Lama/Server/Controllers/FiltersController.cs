using Microsoft.AspNetCore.Http;
using MircoLog.Lama.Shared.Models;

namespace MircoLog.Lama.Server.Controllers;

/* TODO: Move MongoDb specific code to separate service */

[ApiController]
[Route("[controller]")]
public class FiltersController : ControllerBase
{
    private const string COLLECTION_NAME = "filters";
    private readonly ILogger<FiltersController> _logger;

    private MongoConfig _Config { get; set; }
    private IMongoCollection<Filter> _Collection { get; }

    public FiltersController(ILogger<FiltersController> logger, IOptions<MongoConfig> registryConfig)
    {
        _logger = logger;
        _Config = registryConfig.Value;

        var client = new MongoClient(_Config.ConnectionString);
        var database = client.GetDatabase(_Config.DatabaseName);
        _Collection = database.GetCollection<Filter>(COLLECTION_NAME);
    }

    [HttpGet]
    [ActionName("GetAll")]
    public ActionResult<IEnumerable<Filter>> GetAll()
    {
        return _Collection
            .AsQueryable()
            .ToList();
    }

    [HttpPatch("edit")]
    public async Task<IActionResult> Edit([FromBody] Filter filter)
    {
        try
        {
            var filterDefinition = new FilterDefinitionBuilder<Filter>().Where(f => f.Name == filter.Name);
            var updateDefinition = new UpdateDefinitionBuilder<Filter>().Set("Query", filter.Query);
            var result = await _Collection.UpdateOneAsync(filterDefinition, updateDefinition);

            if (result.ModifiedCount >= 1)
            {
                return Ok();
            }
            return BadRequest($"Filter of name {filter.Name} does not exists.");
        }
        catch
        {
            _logger.LogError("Filter - Update failed unexpectedly.", filter);
            return StatusCode(StatusCodes.Status500InternalServerError, "Update failed unexpectedly.");
        }
    }

    [HttpPost("save")]
    public async Task<IActionResult> Save([FromBody] Filter filter)
    {
        try
        {
            var filters = await _Collection.FindAsync(f => f.Name == filter.Name);
            var exists = filters.Any();

            if (exists)
            {
                return BadRequest($"Filter of name {filter.Name} already exists.");
            }

            await _Collection.InsertOneAsync(filter);
            return CreatedAtAction("GetAll", filter);
        }
        catch (Exception ex)
        {
            _logger.LogError("Filter - Save failed unexpectedly.", ex);
            return StatusCode(StatusCodes.Status500InternalServerError, "Save failed unexpectedly.");
        }
    }

    [HttpDelete("{filterName}")]
    public async Task<IActionResult> Delete(string filterName)
    {
        try
        {
            var filterDefinition = new FilterDefinitionBuilder<Filter>().Where(f => f.Name == filterName);
            await _Collection.DeleteOneAsync(filterDefinition);
            return StatusCode(StatusCodes.Status204NoContent);
        }
        catch (Exception ex)
        {
            _logger.LogError("Filter - Delete failed unexpectedly.", ex);
            return StatusCode(StatusCodes.Status500InternalServerError, "Delete failed unexpectedly.");
        }
    }
}
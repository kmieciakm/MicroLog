using Microsoft.AspNetCore.Mvc.Filters;
using MircoLog.Lama.Shared.Models;

namespace MircoLog.Lama.Server.ActionFilters;

public abstract class FilterControllerBase : ControllerBase
{
    protected class FilterExistsAttribute : TypeFilterAttribute
    {
        public FilterExistsAttribute() : base(typeof(FilterExistsAF))
        {
        }
    }

    protected class FilterExistsAF : IAsyncActionFilter
    {
        /* TODO: Redundand with FiltersController */

        private const string COLLECTION_NAME = "filters";
        private MongoConfig _Config { get; set; }
        private IMongoCollection<Filter> _Collection { get; }

        public FilterExistsAF(IOptions<MongoConfig> registryConfig)
        {
            _Config = registryConfig.Value;
            var client = new MongoClient(_Config.ConnectionString);
            var database = client.GetDatabase(_Config.DatabaseName);
            _Collection = database.GetCollection<Filter>(COLLECTION_NAME);
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controller = context.Controller as FilterControllerBase;

            if (context.ActionArguments.TryGetValue("filter", out object value) && value is Filter filter)
            {
                var filters = await _Collection.FindAsync(f => f.Name == filter.Name);
                var exists = filters.Any();

                if (!exists)
                {
                    controller.BadRequest($"Filter of name {filter.Name} does not exists.");
                    return;
                }
            }

            await next();
        }
    }
}
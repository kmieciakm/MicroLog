namespace MircoLog.Lama.Server.GraphQL;

public class Query
{
    [UseOffsetPaging(MaxPageSize = 1000, IncludeTotalCount = true)]
    public async Task<CollectionSegment<Log>> GetLogsFromRepo([Service] ILogRegistry registry, int skip, int take = 100)
    {
        var logsCollection = await registry.GetAsync(skip, take);
        var items = Log
            .Parse(logsCollection.Items)
            .ToList()
            .AsReadOnly();

        var info = new CollectionSegmentInfo(false, false);
        var totalCount = logsCollection.TotalCount;

        return new CollectionSegment<Log>(
            items,
            info,
            _ => ValueTask.FromResult(totalCount)
        );
    }

    [UseOffsetPaging(MaxPageSize = 1000, DefaultPageSize = 100, IncludeTotalCount = true)]
    [UseSorting]
    [UseFiltering]
    public IExecutable<Log> GetLogs([Service] IMongoCollection<Log> collection)
    {
        return collection.AsExecutable();
    }
}

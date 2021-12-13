using HotChocolate.Types;
using HotChocolate.Types.Pagination;

namespace MircoLog.Lama.Server.GraphQL;

public class Query
{
    [UseOffsetPaging(MaxPageSize = 1000, IncludeTotalCount = true)]
    public async Task<CollectionSegment<Log>> GetLogs([Service] ILogRegistry registry, int skip, int take)
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
}

namespace MicroLog.Core;

public class PaginationResult<T> where T : class
{
    public IEnumerable<T> Items { get; set; }
    public int TotalCount { get; set; }
}

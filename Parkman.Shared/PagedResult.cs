namespace Parkman.Shared
{
    public class PagedResult<T>
    {
        public IReadOnlyList<T> Items { get; }
        public int TotalCount { get; }

        public PagedResult(IReadOnlyList<T> items, int totalCount)
        {
            Items = items;
            TotalCount = totalCount;
        }
    }
}

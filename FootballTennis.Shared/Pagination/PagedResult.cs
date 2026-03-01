namespace FootballTennis.Shared.Pagination;

public sealed class PagedResult<T>
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public int TotalCount { get; init; }
    public int TotalPages => PageSize == 0 ? 1 : (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPrevious => Page > 1;
    public bool HasNext => Page < TotalPages;
    public string? Search { get; init; }
    public string? SortBy { get; init; }
    public bool Desc { get; init; }
    public IReadOnlyList<T> Items { get; init; } = [];
}

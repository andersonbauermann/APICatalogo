namespace APICatalogo.Pagination;

public class PaginationParameters
{
    private const int MaxPageSize = 10;
    private static int _pageSize = MaxPageSize;

    public int PageNumber { get; set; } = 1;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
}
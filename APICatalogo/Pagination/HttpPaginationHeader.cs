using Newtonsoft.Json;

namespace APICatalogo.Pagination;

public static class HttpPaginationHeader
{
    public static void AddHeader<T>(HttpResponse response, PagedList<T> properties) where T : class
    {
        var metadata = new
        {
            properties.CurrentPage,
            properties.PageSize,
            properties.TotalCount,
            properties.TotalPages,
            properties.HasPreviousPage,
            properties.HasNextPage
        };
        response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
    }
}
using Newtonsoft.Json;
using X.PagedList;

namespace APICatalogo.Pagination;

public static class HttpPaginationHeader
{
    public static void AddHeader<T>(HttpResponse response, IPagedList<T> properties) where T : class
    {
        var metadata = new
        {
            properties.Count,
            properties.PageSize,
            properties.PageCount,
            properties.TotalItemCount,
            properties.HasPreviousPage,
            properties.HasNextPage
        };
        response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
    }
}
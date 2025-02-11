using APICatalogo.Models;
using APICatalogo.Pagination;
using X.PagedList;

namespace APICatalogo.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<IPagedList<Product>> GetProductsAsync(PaginationParameters paginationParameters);
    Task<IEnumerable<Product>> GetByCategoryAsync(int id);
    Task<IPagedList<Product>> GetFilteredByPriceAsync(ProductsFilterPrice filter);
}
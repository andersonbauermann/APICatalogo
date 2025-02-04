using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories;

public interface IProductRepository : IRepository<Product>
{
    PagedList<Product> GetProducts(PaginationParameters paginationParameters);
    IEnumerable<Product> GetByCategory(int id);
}
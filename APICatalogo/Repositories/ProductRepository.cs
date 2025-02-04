using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories;

public class ProductRepository(AppDbContext context) : Repository<Product>(context), IProductRepository
{
    public PagedList<Product> GetProducts(PaginationParameters paginationParameters)
    {
        var products = GetAll()
            .OrderBy(p => p.Id)
            .AsQueryable();
        var productsPaginated = PagedList<Product>
            .ToPagedList(products, paginationParameters.PageNumber, PaginationParameters.PageSize);
        
        return productsPaginated;
    }

    public IEnumerable<Product> GetByCategory(int id)
    {
        return GetAll().Where(p => p.CategoryId == id);
    }
}
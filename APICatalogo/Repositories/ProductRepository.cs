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
            .ToPagedList(products, paginationParameters.PageNumber, paginationParameters.PageSize);
        
        return productsPaginated;
    }

    public IEnumerable<Product> GetByCategory(int id)
    {
        return GetAll().Where(p => p.CategoryId == id);
    }

    public PagedList<Product> GetFilteredByPrice(ProductsFilterPrice filter)
    {
        var products = GetAll().AsQueryable();
        
        if (filter.Price.HasValue && Enum.IsDefined(filter.CriterionPrice))
        {
            products = filter.CriterionPrice switch
            {
                ProductsFilterPrice.CriterionPriceEnum.BIGGER_THAN => 
                    products.Where(p => p.Price > filter.Price.Value)
                    .OrderBy(p => p.Price),
                
                ProductsFilterPrice.CriterionPriceEnum.SMALLER_THAN => 
                    products.Where(p => p.Price < filter.Price.Value)
                    .OrderBy(p => p.Price),
                
                ProductsFilterPrice.CriterionPriceEnum.EQUAL => 
                    products.Where(p => p.Price == filter.Price.Value)
                    .OrderBy(p => p.Price),
                
                _ => Enumerable.Empty<Product>().AsQueryable()
            };
        }
        var productsPaginated = PagedList<Product>
            .ToPagedList(products, filter.PageNumber, filter.PageSize);
            
        return productsPaginated;
    }
}
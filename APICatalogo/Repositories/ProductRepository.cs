using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using X.PagedList;
using X.PagedList.Extensions;

namespace APICatalogo.Repositories;

public class ProductRepository(AppDbContext context) : Repository<Product>(context), IProductRepository
{
    public async Task<IPagedList<Product>> GetProductsAsync(PaginationParameters paginationParameters)
    {
        var products = await GetAllAsync();
        var sortedProducts = products  
            .OrderBy(p => p.Id)
            .AsQueryable();
        //var productsPaginated = Pagination.PagedList<Product>
        //    .ToPagedList(sortedProducts, paginationParameters.PageNumber, paginationParameters.PageSize);
        var productsPaginated = sortedProducts.ToPagedList(paginationParameters.PageNumber, paginationParameters.PageSize);
        return productsPaginated;
    }

    public async Task<IEnumerable<Product>> GetByCategoryAsync(int id)
    {
        var products = await GetAllAsync();
        return products.Where(p => p.CategoryId == id);
    }

    public async Task<IPagedList<Product>> GetFilteredByPriceAsync(ProductsFilterPrice filter)
    {
        var products = await GetAllAsync();
        
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
        //var productsPaginated = Pagination.PagedList<Product>
        //    .ToPagedList(products.AsQueryable(), filter.PageNumber, filter.PageSize);
        var pageProducts = products.ToPagedList(filter.PageNumber, filter.PageSize);
        return pageProducts;
    }
}
using APICatalogo.Models;
using APICatalogo.Pagination;
using X.PagedList;

namespace APICatalogo.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    public Task<IPagedList<Category>> GetCategoriesAsync(PaginationParameters paginationParameters);
    public Task<IPagedList<Category>> GetCategoriesByNameAsync(CategoryFilterName parameters);
}
using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories;

public class CategoryRepository(AppDbContext context) : Repository<Category>(context), ICategoryRepository
{
    public PagedList<Category> GetCategeories(PaginationParameters paginationParameters)
    {
        var categories = GetAll()
            .OrderBy(p => p.Id)
            .AsQueryable();
        var categoriesPaginated = PagedList<Category>
            .ToPagedList(categories, paginationParameters.PageNumber, PaginationParameters.PageSize);
        
        return categoriesPaginated;
    }
}
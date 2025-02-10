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
            .ToPagedList(categories, paginationParameters.PageNumber, paginationParameters.PageSize);
        
        return categoriesPaginated;
    }

    public PagedList<Category> GetCategeoriesByName(CategoryFilterName parameters)
    {
        var categories = GetAll().AsQueryable();
        
        if (!string.IsNullOrEmpty(parameters.Name)) 
            categories = categories.Where(c => c.Name.Contains(parameters.Name));
        
        var filteredCategories = PagedList<Category>
            .ToPagedList(categories, parameters.PageNumber, parameters.PageSize);
        
        return filteredCategories;
    }
}
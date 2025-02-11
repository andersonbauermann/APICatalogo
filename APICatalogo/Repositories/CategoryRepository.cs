using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.Extensions;

namespace APICatalogo.Repositories;

public class CategoryRepository(AppDbContext context) : Repository<Category>(context), ICategoryRepository
{
    public async Task<IPagedList<Category>> GetCategoriesAsync(PaginationParameters paginationParameters)
    {
        var categories = await GetAllAsync();
        var sortedCategories = categories
            .OrderBy(p => p.Id)
            .AsQueryable();
        
        //var categoriesPage = Pagination.PagedList<Category>
        //    .ToPagedList(sortedCategories, paginationParameters.PageNumber, paginationParameters.PageSize);
        var categoriesPage = sortedCategories.ToPagedList(paginationParameters.PageNumber, paginationParameters.PageSize);
        return categoriesPage;
    }

    public async Task<IPagedList<Category>> GetCategoriesByNameAsync(CategoryFilterName parameters)
    {
        var categories = await GetAllAsync();
        
        if (!string.IsNullOrEmpty(parameters.Name)) 
            categories = categories.Where(c => c.Name.Contains(parameters.Name));
        
        //var filteredCategories = Pagination.PagedList<Category>
        //    .ToPagedList(categories.AsQueryable(), parameters.PageNumber, parameters.PageSize);
        var filteredCategories = categories.ToPagedList(parameters.PageNumber, parameters.PageSize);
        
        return filteredCategories;
    }
}
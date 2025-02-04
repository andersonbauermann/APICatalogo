using APICatalogo.Models;

namespace APICatalogo.DTOs.Mappings;

public static class CategoryDtoMappingExtensions
{
    public static CategoryDTO? ToCategoryDto(this Category category)
    {
        if (category is null) return null;

        return new CategoryDTO
        {
            Id = category.Id,
            Name = category.Name,
            ImageUrl = category.ImageUrl
        };
    }

    public static Category? ToCategory(this CategoryDTO categoryDto)
    {
        if (categoryDto is null) return null;

        return new Category
        {
            Id = categoryDto.Id,
            Name = categoryDto.Name,
            ImageUrl = categoryDto.ImageUrl
        };
    }

    public static IEnumerable<CategoryDTO> ToCategoryDtoList(this IEnumerable<Category> categories)
    {
        if (categories is null || !categories.Any()) return [];
        
        return categories.Select(category => new CategoryDTO
        {
            Id = category.Id,
            Name = category.Name,
            ImageUrl = category.ImageUrl
        });
    }
}
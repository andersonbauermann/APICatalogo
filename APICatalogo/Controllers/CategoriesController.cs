using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Filters;
using APICatalogo.Pagination;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(ILogger<CategoriesController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAll()
    {
        var categories = await _unitOfWork.CategoryRepository.GetAllAsync();

        if (categories is null) return NotFound("Nenhuma categoria encontrada");

        var categoriesDto = categories.ToCategoryDtoList();

        return Ok(categoriesDto);
    }
    
    [HttpGet("pagination")]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAllPagination([FromQuery] PaginationParameters paginationParameters)
    {
        var categories = await _unitOfWork.CategoryRepository.GetCategoriesAsync(paginationParameters);

        HttpPaginationHeader.AddHeader(Response, categories);
        
        var categoriesDto = categories.ToCategoryDtoList();
        
        return Ok(categoriesDto);
    }
    
    [HttpGet("filter/name/pagination")]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAllPaginationFiltered([FromQuery] CategoryFilterName parameters)
    {
        var categories = await _unitOfWork.CategoryRepository.GetCategoriesByNameAsync(parameters);

        HttpPaginationHeader.AddHeader(Response, categories);
        
        var categoriesDto = categories.ToCategoryDtoList();
        
        return Ok(categoriesDto);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult<CategoryDTO>> GetById(int id)
    {
        var category = await _unitOfWork.CategoryRepository.GetAsync(category => category.Id == id);

        if (category is not null)
        {
            var categoryDto = category.ToCategoryDto();
            
            return Ok(categoryDto);
        }

        _logger.LogWarning($"Nenhuma categoria encontrada: {id}");
        return NotFound("Nenhuma categoria encontrada");
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDTO>> Create(CategoryDTO categoryDto)
    {
        if (categoryDto is null)
        {
            _logger.LogWarning($"Dados da categoria inválidos.");
            return BadRequest("Dados da categoria inválidos.");
        }

        var category = categoryDto.ToCategory();

        var createdCategory = _unitOfWork.CategoryRepository.Create(category);
        await _unitOfWork.CommitAsync();
        
        var newCategoryDto = createdCategory.ToCategoryDto();

        return CreatedAtRoute(nameof(GetById), new { id = newCategoryDto.Id }, newCategoryDto);
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<ActionResult<CategoryDTO>> Update(int id, CategoryDTO categoryDto)
    {
        if (id != categoryDto.Id)
        {
            _logger.LogWarning($"Dados da categoria inválidos - {categoryDto}");
            return BadRequest("Dados da categoria inválidos.");
        }
        
        var category = categoryDto.ToCategory();

        var updatedCategory = _unitOfWork.CategoryRepository.Update(category);
       await  _unitOfWork.CommitAsync();
        
        var updatedCategoryDto = updatedCategory.ToCategoryDto();
        
        return Ok(updatedCategoryDto);
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult<CategoryDTO>> Delete(int id)
    {
        var category = await _unitOfWork.CategoryRepository.GetAsync(category => category.Id == id);

        if (category is null) return NotFound();

        var deletedCategory = _unitOfWork.CategoryRepository.Delete(category);
        await _unitOfWork.CommitAsync();
        
        var deletedCategoryDto = deletedCategory.ToCategoryDto();
        
        return Ok(deletedCategoryDto);
    }
}
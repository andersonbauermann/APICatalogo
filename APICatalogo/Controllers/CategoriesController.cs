using APICatalogo.Filters;
using APICatalogo.Models;
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
    public ActionResult<IEnumerable<Category>> GetAll()
    {
        var categories = _unitOfWork.CategoryRepository.GetAll();

        if (categories is null) return NotFound("Nenhuma categoria encontrada");

        return Ok(categories);
    }

    [HttpGet("{id:int:min(1)}")]
    public ActionResult<Category> GetById(int id)
    {
        var category = _unitOfWork.CategoryRepository.Get(category => category.Id == id);

        if (category is not null) return Ok(category);

        _logger.LogWarning($"Nenhuma categoria encontrada: {id}");
        return NotFound("Nenhuma categoria encontrada");
    }

    [HttpPost]
    public ActionResult Create(Category category)
    {
        if (category is null)
        {
            _logger.LogWarning($"Dados da categoria inválidos - {category}");
            return BadRequest("Dados da categoria inválidos.");
        }

        var createdCategory = _unitOfWork.CategoryRepository.Create(category);
        _unitOfWork.Commit();

        return CreatedAtRoute(nameof(GetById), new { id = createdCategory.Id }, createdCategory);
    }

    [HttpPut("{id:int:min(1)}")]
    public ActionResult Update(int id, Category category)
    {
        if (id != category.Id)
        {
            _logger.LogWarning($"Dados da categoria inválidos - {category}");
            return BadRequest("Dados da categoria inválidos.");
        }

        _unitOfWork.CategoryRepository.Update(category);
        _unitOfWork.Commit();
        return Ok();
    }

    [HttpDelete("{id:int:min(1)}")]
    public ActionResult Delete(int id)
    {
        var category = _unitOfWork.CategoryRepository.Get(category => category.Id == id);

        if (category is null) return NotFound();

        var deletedCategory = _unitOfWork.CategoryRepository.Delete(category);
        _unitOfWork.Commit();
        return Ok(deletedCategory);
    }
}
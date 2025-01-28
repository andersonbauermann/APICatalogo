using APICatalogo.Context;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryRepository _repository;
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(ICategoryRepository repository, ILogger<CategoriesController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public ActionResult<IEnumerable<Category>> GetAll()
    {
        var categories = _repository.GetAll();

        if (categories is null) return NotFound("Nenhuma categoria encontrada");

        return Ok(categories);
    }

    [HttpGet("{id:int:min(1)}")]
    public ActionResult<Category> GetById(int id)
    {
        var category = _repository.Get(category => category.Id == id);

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

        var createdCategory = _repository.Create(category);

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

        _repository.Update(category);
        return Ok();
    }

    [HttpDelete("{id:int:min(1)}")]
    public ActionResult Delete(int id)
    {
        var category = _repository.Get(category => category.Id == id);

        if (category is null) return NotFound();

        var deletedCategory = _repository.Delete(category);
        return Ok(deletedCategory);
    }
}
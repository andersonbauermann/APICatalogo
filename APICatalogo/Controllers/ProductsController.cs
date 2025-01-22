using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _repository;

    public ProductsController(IProductRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetAll()
    {
        var products = _repository.GetProducts().ToList();
        
        if (products is null) return NotFound("Nenhum produto encontrado");
        
        return Ok(products);
    }

    [HttpGet("{id:int:min(1)}", Name = "GetProductById")]
    public ActionResult<Product> GetById(int id)
    {
        var product = _repository.GetProduct(id);
        
        if (product is null) return NotFound("Nenhum produto encontrado");
        
        return product;
    }

    [HttpPost]
    public ActionResult Create(Product product)
    {
        if (product is null) return BadRequest();
        
        var newProduct = _repository.Create(product);
        
        return CreatedAtRoute(nameof(GetById), new { id = product.Id }, newProduct);
    }

    [HttpPut("{id:int:min(1)}")]
    public ActionResult Update(int id, Product product)
    {
        if (id != product.Id) return BadRequest();
        
        bool successOnUpdate = _repository.Update(product);
        
        if (successOnUpdate) return Ok(product);
        
        return StatusCode(304, "Falha ao atualizar o produto.");
    }

    [HttpDelete("{id:int:min(1)}")]
    public ActionResult Delete(int id)
    {
        bool successOnDelete = _repository.Delete(id);
        
        if (successOnDelete) return Ok($"Produto de ID -{id}- foi deletado.");
        
        return StatusCode(304, "Falha ao deletar o produto.");
    }
}
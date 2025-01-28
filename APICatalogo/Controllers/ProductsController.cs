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
    private readonly IUnitOfWork _unitOfWork;

    public ProductsController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetAll()
    {
        var products = _unitOfWork.ProductRepository.GetAll();

        if (products is null) return NotFound("Nenhum produto encontrado");

        return Ok(products);
    }

    [HttpGet("{id:int:min(1)}", Name = "GetProductById")]
    public ActionResult<Product> GetById(int id)
    {
        var product = _unitOfWork.ProductRepository.Get(product => product.Id == id);

        if (product is null) return NotFound("Nenhum produto encontrado");

        return product;
    }

    [HttpPost("products/{id:int}")]
    public ActionResult<IEnumerable<Product>> GetProductsCategory(int id)
    {
        var products = _unitOfWork.ProductRepository.GetByCategory(id);

        if (products is null) return NotFound("Nenhum produto encontrado");

        return Ok(products);
    }

    [HttpPost]
    public ActionResult Create(Product product)
    {
        if (product is null) return BadRequest();

        var newProduct = _unitOfWork.ProductRepository.Create(product);
        _unitOfWork.Commit();

        return CreatedAtRoute(nameof(GetById), new { id = product.Id }, newProduct);
    }

    [HttpPut("{id:int:min(1)}")]
    public ActionResult Update(int id, Product product)
    {
        if (id != product.Id) return BadRequest();

        var updatedProduct = _unitOfWork.ProductRepository.Update(product);
        _unitOfWork.Commit();

        return updatedProduct is not null ? Ok(product) : StatusCode(304, "Falha ao atualizar o produto.");
    }

    [HttpDelete("{id:int:min(1)}")]
    public ActionResult Delete(int id)
    {
        var product = _unitOfWork.ProductRepository.Get(product => product.Id == id);

        if (product is null) return NotFound("Falha ao deletar o produto.");

        _unitOfWork.ProductRepository.Delete(product);
        _unitOfWork.Commit();
        return Ok($"Produto de ID -{id}- foi deletado.");
    }
}
using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Newtonsoft.Json;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
//[EnableRateLimiting("fixed_window_limit")]
public class ProductsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProductsController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAll()
    {
        var products = await _unitOfWork.ProductRepository.GetAllAsync();

        if (products is null) return NotFound("Nenhum produto encontrado");

        var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);
        return Ok(productsDto);
    }

    [HttpGet("pagination")]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllPagination([FromQuery] PaginationParameters paginationParameters)
    {
        var products = await _unitOfWork.ProductRepository.GetProductsAsync(paginationParameters);

        HttpPaginationHeader.AddHeader(Response, products);
        
        var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);
        
        return Ok(productsDto);
    }
    
    [HttpGet("/filter/price/pagination")]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllPagination([FromQuery] ProductsFilterPrice productFilter)
    {
        var products = await _unitOfWork.ProductRepository.GetFilteredByPriceAsync(productFilter);

        HttpPaginationHeader.AddHeader(Response, products);
        
        var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);
        
        return Ok(productsDto);
    }

    [HttpGet("{id:int:min(1)}", Name = "GetProductById")]
    public async Task<ActionResult<ProductDTO>> GetById(int id)
    {
        var product = await _unitOfWork.ProductRepository.GetAsync(product => product.Id == id);

        if (product is null) return NotFound("Nenhum produto encontrado");

        var productDto = _mapper.Map<ProductDTO>(product);
        return Ok(productDto);
    }

    [HttpPost("products/{id:int}")]
    public ActionResult<IEnumerable<ProductDTO>> GetProductsCategory(int id)
    {
        var products = _unitOfWork.ProductRepository.GetByCategoryAsync(id);

        if (products is null) return NotFound("Nenhum produto encontrado");

        var produtsDtoList = _mapper.Map<IEnumerable<ProductDTO>>(products);
        return Ok(produtsDtoList);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDTO>> Create(ProductDTO productDto)
    {
        if (productDto is null) return BadRequest();
        
        var product = _mapper.Map<Product>(productDto);
        var newProduct = _unitOfWork.ProductRepository.Create(product);
        await _unitOfWork.CommitAsync();
        
        var newProductDto = _mapper.Map<ProductDTO>(newProduct);
        return CreatedAtRoute(nameof(GetById), new { id = product.Id }, newProductDto);
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<ActionResult<ProductDTO>> Update(int id, ProductDTO productDto)
    {
        if (id != productDto.Id) return BadRequest();

        var product = _mapper.Map<Product>(productDto);
        var updatedProduct = _unitOfWork.ProductRepository.Update(product);
        await _unitOfWork.CommitAsync();

        var updatedProductDto = _mapper.Map<ProductDTO>(updatedProduct);
        return updatedProduct is not null ? Ok(updatedProductDto) : StatusCode(304, "Falha ao atualizar o produto.");
    }

    [HttpPatch("{id:int:min(1)}/updatePartial")]
    public async Task<ActionResult<ProductDTOUpdateResponse>> UpdatePartial(int id, JsonPatchDocument<ProductDTOUpdateRequest> productDto)
    {
        if (productDto is null || id <= 0) return BadRequest();
        
        var product = await _unitOfWork.ProductRepository.GetAsync(product => product.Id == id);

        if (product is null) return NotFound();
        
        var productUpdateRequest = _mapper.Map<ProductDTOUpdateRequest>(product);
        productDto.ApplyTo(productUpdateRequest, ModelState);
        
        if (!ModelState.IsValid || TryValidateModel(product)) return BadRequest(ModelState);
        
        _mapper.Map(productUpdateRequest, product);
        _unitOfWork.ProductRepository.Update(product);
        await _unitOfWork.CommitAsync();
        
        var productDtoUpdateResponse = _mapper.Map<ProductDTOUpdateResponse>(product);
        return Ok(productDtoUpdateResponse);
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult<ProductDTO>> Delete(int id)
    {
        var product = await _unitOfWork.ProductRepository.GetAsync(product => product.Id == id);

        if (product is null) return NotFound("Falha ao deletar o produto.");

        _unitOfWork.ProductRepository.Delete(product);
        await _unitOfWork.CommitAsync();
        
        var deletedProductDto = _mapper.Map<ProductDTO>(product);
        return Ok(deletedProductDto);
    }
}
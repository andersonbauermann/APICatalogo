using APICatalogo.Controllers;
using APICatalogo.DTOs;
using APICatalogo.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogoTest.UnitTests;

public class GetProductUnitTest : IClassFixture<ProductsUnitTestController>
{
    private readonly ProductsController _controller;

    public GetProductUnitTest(ProductsUnitTestController controller)
    {
        _controller = new ProductsController(controller.repository, controller.mapper);
    }

    [Fact]
    public async Task GetProductById_Return_OkResult()
    {
        //Arrange
        const int productId = 1;
        //Act
        var data = await _controller.GetById(productId);
        //Assert
        var okResult = Assert.IsType<OkObjectResult>(data.Result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.IsAssignableFrom<ProductDTO>(okResult.Value);
        
        //(fluentAssertions)
        // data.Result.Should().BeOfType<OkObjectResult>()
        //     .Which.StatusCode.Should().Be(200);
    }
    
    [Fact]
    public async Task GetProductById_Return_NotFound()
    {
        //Arrange
        const int productId = 0;
        //Act
        var data = await _controller.GetById(productId);
        //Assert
        var result = Assert.IsType<NotFoundObjectResult>(data.Result);
        Assert.Equal(404, result.StatusCode);
        Assert.IsNotAssignableFrom<ProductDTO>(result.Value);
    }
}
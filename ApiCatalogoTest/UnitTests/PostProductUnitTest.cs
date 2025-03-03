using APICatalogo.Controllers;
using APICatalogo.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogoTest.UnitTests;

public class PostProductUnitTest : IClassFixture<ProductsUnitTestController>
{
    private readonly ProductsController _controller;

    public PostProductUnitTest(ProductsUnitTestController controller)
    {
        _controller = new ProductsController(controller.repository, controller.mapper);
    }

    [Fact]
    public async Task PostCreate_Return_Created()
    {
        //Arrange
        var newProduct = new ProductDTO()
        {
            Name = "Novo produto",
            Description = "Descrição Novo produto",
            Price = 10m,
            ImageUrl = "imagem_tst.jpg",
            CategoryId = 1
        };
        //Act
        var data = await _controller.Create(newProduct);
        //Assert
        Assert.NotNull(data.Result);
        var result = Assert.IsType<CreatedAtActionResult>(data.Result);
        Assert.IsAssignableFrom<ProductDTO>(data.Result);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task PostCreate_Return_BadRequest()
    {
        //Arrange
        ProductDTO product = null;
        //Act
        var data = await _controller.Create(product);
        //Assert
        var badRequestResult = data.Result.Should().BeOfType<BadRequestResult>();
        badRequestResult.Subject.StatusCode.Should().Be(400);
    }
}
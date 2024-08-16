using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;


public class ProductsControllerTests
{
    private readonly Mock<AppDbContext> _mockContext;
    private readonly Mock<IWebHostEnvironment> _mockEnvironment;
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _mockContext = new Mock<AppDbContext>();
        _mockEnvironment = new Mock<IWebHostEnvironment>();

        _controller = new ProductsController(_mockContext.Object, _mockEnvironment.Object);
    }

    [Fact]
    public void Index_ReturnsViewResult_WithListOfProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product { Id = 1, Name = "Product1", Price = 10.00M, Image = "image1.jpg" },
            new Product { Id = 2, Name = "Product2", Price = 20.00M, Image = "image2.jpg" }
        }.AsQueryable();

        var mockSet = new Mock<DbSet<Product>>();
        mockSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(products.Provider);
        mockSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(products.Expression);
        mockSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(products.ElementType);
        mockSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(products.GetEnumerator());

        _mockContext.Setup(c => c.Products).Returns(mockSet.Object);

        // Act
        var result = _controller.Index() as ViewResult;

        // Assert
        var model = Assert.IsType<List<Product>>(result?.ViewData.Model);
        Assert.Equal(2, model.Count);
    }

    [Fact]
    public void Create_Post_ReturnsViewResult_WhenModelIsInvalid()
    {
        // Arrange
        _controller.ModelState.AddModelError("Name", "Name is required");

        var productDto = new ProductDto
        {
            Name = "",
            Price = 10.00M
        };

        // Act
        var result = _controller.Create(productDto) as ViewResult;

        // Assert
        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
    }

    [Fact]
    public void Create_Post_RedirectsToIndex_WhenModelIsValid()
    {
        // Arrange
        var productDto = new ProductDto
        {
            Name = "Product1",
            Price = 10.00M,
            Image = Mock.Of<IFormFile>(f => f.FileName == "image.jpg")
        };

        _mockEnvironment.Setup(e => e.WebRootPath).Returns("wwwroot");

        // Act
        var result = _controller.Create(productDto) as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
        Assert.Equal("Products", result.ControllerName);
    }

    [Fact]
    public void Edit_Post_ReturnsRedirectToIndex_WhenModelIsValid()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Product1", Price = 10.00M, Image = "image1.jpg" };
        _mockContext.Setup(c => c.Products.Find(1)).Returns(product);

        var productDto = new ProductDto { Name = "Updated Product", Price = 15.00M };

        // Act
        var result = _controller.Edit(1, productDto) as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
    }

    [Fact]
    public void Delete_RedirectsToIndex_WhenProductIsDeleted()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Product1", Price = 10.00M, Image = "image1.jpg" };
        _mockContext.Setup(c => c.Products.Find(1)).Returns(product);
        _mockEnvironment.Setup(e => e.WebRootPath).Returns("wwwroot");

        // Act
        var result = _controller.Delete(1) as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
    }
}

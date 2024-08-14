public class HomeControllerTests
{
    private readonly HomeController _controller;
    private readonly Mock<AppDbContext> _mockContext;
    private readonly Mock<DbSet<Product>> _mockProductsSet;

   public HomeControllerTests()
    {
        _mockContext = new Mock<AppDbContext>();
        _mockProductsSet = new Mock<DbSet<Product>>();

        _mockContext.Setup(c => c.Products).Returns(_mockProductsSet.Object);

        _controller = new HomeController(_mockContext.Object);
    }

    [Fact]
public void Index_ReturnsViewResult_WithListOfProducts()
{
    // Arrange
    var products = new List<Product>
    {
        new Product { Id = 1, Name = "Product1", Image = "image1.jpg", Price = 10.00M },
        new Product { Id = 2, Name = "Product2", Image = "image2.jpg", Price = 20.00M }
    }.AsQueryable();

    _mockProductsSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(products.Provider);
    _mockProductsSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(products.Expression);
    _mockProductsSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(products.ElementType);
    _mockProductsSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(products.GetEnumerator());

    // Act
    var result = _controller.Index() as ViewResult;

    // Assert
    var model = Assert.IsType<List<Product>>(result?.ViewData.Model);
    Assert.Equal(2, model.Count);
    Assert.Equal("Product1", model[0].Name);
    Assert.Equal("image1.jpg", model[0].Image);
    Assert.Equal(10.00M, model[0].Price);
    Assert.Equal("Product2", model[1].Name);
    Assert.Equal("image2.jpg", model[1].Image);
    Assert.Equal(20.00M, model[1].Price);
}

[Fact]
public void Shop_ReturnsViewResult_WithListOfProducts()
{
    // Arrange
    var products = new List<Product>
    {
        new Product { Id = 1, Name = "Product1", Image = "image1.jpg", Price = 10.00M },
        new Product { Id = 2, Name = "Product2", Image = "image2.jpg", Price = 20.00M }
    }.AsQueryable();

    _mockProductsSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(products.Provider);
    _mockProductsSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(products.Expression);
    _mockProductsSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(products.ElementType);
    _mockProductsSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(products.GetEnumerator());

    // Act
    var result = _controller.Shop() as ViewResult;

    // Assert
    var model = Assert.IsType<List<Product>>(result?.ViewData.Model);
    Assert.Equal(2, model.Count);
    Assert.Equal("Product1", model[0].Name);
    Assert.Equal("image1.jpg", model[0].Image);
    Assert.Equal(10.00M, model[0].Price);
    Assert.Equal("Product2", model[1].Name);
    Assert.Equal("image2.jpg", model[1].Image);
    Assert.Equal(20.00M, model[1].Price);
}

[Fact]
public void About_ReturnsViewResult()
{
    // Act
    var result = _controller.About() as ViewResult;

    // Assert
    Assert.NotNull(result);
}

[Fact]
public void Blog_ReturnsViewResult()
{
    // Act
    var result = _controller.Blog() as ViewResult;

    // Assert
    Assert.NotNull(result);
}

[Fact]
public void Contact_ReturnsViewResult()
{
    // Act
    var result = _controller.Contact() as ViewResult;

    // Assert
    Assert.NotNull(result);
}

[Fact]
public void About_ReturnsViewResult()
{
    // Act
    var result = _controller.About() as ViewResult;

    // Assert
    Assert.NotNull(result);
}

[Fact]
public void Blog_ReturnsViewResult()
{
    // Act
    var result = _controller.Blog() as ViewResult;

    // Assert
    Assert.NotNull(result);
}

[Fact]
public void Contact_ReturnsViewResult()
{
    // Act
    var result = _controller.Contact() as ViewResult;

    // Assert
    Assert.NotNull(result);
}


[Fact]
public void Error_ReturnsViewResult_WithErrorViewModel()
{
    // Act
    var result = _controller.Error() as ViewResult;

    // Assert
    var model = Assert.IsType<ErrorViewModel>(result?.ViewData.Model);
    Assert.NotNull(model.RequestId);
}

}

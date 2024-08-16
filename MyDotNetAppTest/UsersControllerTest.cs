using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

public class UsersControllerTests
{
    private readonly Mock<AppDbContext> _mockContext;
    private readonly UsersController _controller;

    public UsersControllerTests()
    {
        _mockContext = new Mock<AppDbContext>();
        _controller = new UsersController(_mockContext.Object);
    }

    [Fact]
    public void Index_ReturnsViewResult_WithListOfUsers()
    {
        // Arrange
        var users = new List<User>
        {
            new User { Id = 1, Name = "User1", Email = "user1@example.com", Password = "password1", Role = "Admin" },
            new User { Id = 2, Name = "User2", Email = "user2@example.com", Password = "password2", Role = "User" }
        }.AsQueryable();

        var mockSet = new Mock<DbSet<User>>();
        mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
        mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
        mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
        mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

        _mockContext.Setup(c => c.Users).Returns(mockSet.Object);

        // Act
        var result = _controller.Index() as ViewResult;

        // Assert
        var model = Assert.IsType<List<User>>(result?.ViewData.Model);
        Assert.Equal(2, model.Count);
    }

    [Fact]
    public void Create_Post_ReturnsViewResult_WhenModelIsInvalid()
    {
        // Arrange
        _controller.ModelState.AddModelError("Name", "Name is required");

        var userDto = new UserDto
        {
            Name = "",
            Email = "user@example.com",
            Password = "password",
            Role = "User"
        };

        // Act
        var result = _controller.Create(userDto) as ViewResult;

        // Assert
        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
    }

    [Fact]
    public void Create_Post_RedirectsToIndex_WhenModelIsValid()
    {
        // Arrange
        var userDto = new UserDto
        {
            Name = "User1",
            Email = "user1@example.com",
            Password = "password1",
            Role = "Admin"
        };

        // Act
        var result = _controller.Create(userDto) as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
    }

    [Fact]
    public void Edit_Post_RedirectsToIndex_WhenModelIsValid()
    {
        // Arrange
        var user = new User { Id = 1, Name = "User1", Email = "user1@example.com", Password = "password1", Role = "Admin" };
        _mockContext.Setup(c => c.Users.Find(1)).Returns(user);

        var userDto = new UserDto { Name = "Updated User", Email = "updated@example.com", Password = "newpassword", Role = "User" };

        // Act
        var result = _controller.Edit(1, userDto) as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
    }

    [Fact]
    public void Delete_RedirectsToIndex_WhenUserIsDeleted()
    {
        // Arrange
        var user = new User { Id = 1, Name = "User1", Email = "user1@example.com", Password = "password1", Role = "Admin" };
        _mockContext.Setup(c => c.Users.Find(1)).Returns(user);

        // Act
        var result = _controller.Delete(1) as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
    }
}

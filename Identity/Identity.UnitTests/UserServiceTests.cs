using Moq;
using Identity.Core;
using Identity.Data;


namespace Identity.UnitTests;

public class UsersServiceTests
{
    [Fact]
    public async Task AddAsync_CallsAddOnRepository_WithUser()
    {
        // Arrange
        var mockRepo = new Mock<IUsersRepository>();
        var service = new UsersService(mockRepo.Object);
        var userDto = new UserDto {Name = "Test User", Email = "test@example.com"};

        // Act
        await service.AddAsync(userDto);

        // Assert
        mockRepo.Verify(repo => repo.AddAsync(It.Is<User>(u => u.Name == userDto.Name
                                                               && u.Email == userDto.Email)), Times.Once);
    }

    [Fact]
    public async Task ListAsync_ReturnsListOfUserDtos()
    {
        // Arrange
        var users = new List<User>
        {
            new User {Id = 1, Name = "Test User 1", Email = "test1@example.com"},
            new User {Id = 2, Name = "Test User 2", Email = "test2@example.com"}
        };

        var mockRepo = new Mock<IUsersRepository>();
        mockRepo.Setup(repo => repo.ListAsync()).ReturnsAsync(users);
        var service = new UsersService(mockRepo.Object);

        // Act
        var result = await service.ListAsync();

        // Assert
        Assert.Equal(users.Count, result.Count);
        Assert.Equal(users[0].Name, result[0].Name);
        Assert.Equal(users[1].Email, result[1].Email);
    }

    [Fact]
    public async Task FindOrCreateUser_ReturnsExistingUser_WhenUserExists()
    {
        // Arrange
        var mockRepo = new Mock<IUsersRepository>();
        var service = new UsersService(mockRepo.Object);
        const string name = "Test User";
        const string email = "test@example.com";
        var existingUser = new User {Id = 100, Email = email, Name = name};

        await service.FindOrCreateUser(name, email);

        mockRepo.Setup(repo => repo.FindByEmailAsync(email)).ReturnsAsync(existingUser);
        mockRepo.Setup(repo => repo.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask).Verifiable();

        // Act        
        var result = await service.FindOrCreateUser(email, name);

        // Assert
        mockRepo.Verify(repo => repo.FindByEmailAsync(email), Times.Once);
        Assert.Equal(email, result.Email);
        Assert.Equal(name, result.Name);
        Assert.Equal(existingUser.Id, result.Id);
    }

    [Fact]
    public async Task FindOrCreateUser_CreatesNewUser_WhenUserDoesNotExist()
    {
        // Arrange
        var mockRepo = new Mock<IUsersRepository>();
        const string email = "new@example.com";
        const string name = "New User";
        User? nullUser = null;

        mockRepo.Setup(repo => repo.FindByEmailAsync(email))!.ReturnsAsync(nullUser);
        mockRepo.Setup(repo => repo.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask).Verifiable();
        var service = new UsersService(mockRepo.Object);

        // Act
        var result = await service.FindOrCreateUser(email, name);

        // Assert
        mockRepo.Verify(repo => repo.FindByEmailAsync(email), Times.Once);
        mockRepo.Verify(repo => repo.AddAsync(It.Is<User>(u => u.Email == email && u.Name == name)), Times.Once);
        Assert.Equal(email, result.Email);
        Assert.Equal(name, result.Name);

    }
    
    [Fact]
    public async Task MakeAdminAsync_DoesNotChangeUser_WhenPasswordIsIncorrect()
    {
        // Arrange
        var mockRepo = new Mock<IUsersRepository>();
        var service = new UsersService(mockRepo.Object);
        var email = "user@example.com";
        var incorrectPassword = "wrong";

        // Act
        await service.MakeAdminAsync(email, incorrectPassword);

        // Assert
        mockRepo.Verify(repo => repo.ChangeUserAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task MakeAdminAsync_ChangesUser_WhenPasswordIsCorrect()
    {
        // Arrange
        var mockRepo = new Mock<IUsersRepository>();
        var service = new UsersService(mockRepo.Object);
        var email = "user@example.com";
        var correctPassword = "admin";

        mockRepo.Setup(repo => repo.ChangeUserAsync(email)).Returns(Task.CompletedTask).Verifiable();

        // Act
        await service.MakeAdminAsync(email, correctPassword);

        // Assert
        mockRepo.Verify(repo => repo.ChangeUserAsync(email), Times.Once);
    }
}
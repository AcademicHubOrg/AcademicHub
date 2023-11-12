using Xunit;
using Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Identity.Core;
using System.Threading.Tasks;
using System.Linq;


namespace Identity.IntegrationTests
{
    public class UsersIntegrationTest
    {
        private DbContextOptions<IdentityDbContext> CreateNewContextOptions()
        {
            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<IdentityDbContext>();
            builder.UseInMemoryDatabase("TestDataBase")
                .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }

        [Fact]
        public async Task UsersRepository_AddAsync_AddUserToDatabase()
        {
            // Arrange
            var options = CreateNewContextOptions();

            // Use a clean instance of the context to run the test
            await using (var context = new IdentityDbContext(options))
            {
                var repository = new UsersRepository(context);
                var user = new User { Name = "Test User", Email = "test@example.com" };

                // Act
                await repository.AddAsync(user);
            }

            // Assert
            await using (var context = new IdentityDbContext(options))
            {
                Assert.Equal(1, await context.Users.CountAsync());
                var dbUser = await context.Users.FirstOrDefaultAsync(u => u.Email == "test@example.com");
                Assert.NotNull(dbUser);
                Assert.Equal("Test User", dbUser.Name);
            }
        }
        

        [Fact]
        public async Task UsersRepository_ChangeUserAsync_ChangeUsersIsAdminToTrue()
        {
            // Arrange
            var options = CreateNewContextOptions();

            // Use a clean instance of the context to run the test
            await using (var context = new IdentityDbContext(options))
            {
                var repository = new UsersRepository(context);
                var user = new User { Name = "Test User", Email = "test@example.com" };

                // Act
                await repository.AddAsync(user);
                await repository.ChangeUserAsync(user.Email);
            }

            // Assert
            await using (var context = new IdentityDbContext(options))
            {
                var dbUser = await context.Users.FirstOrDefaultAsync(u => u.Email == "test@example.com");
                Assert.NotNull(dbUser);
                Assert.True(dbUser.IsAdmin);
            }
        }
        
        [Fact]
        public async Task UsersService_AddAsync_CallsAddOnRepository_WithUser()
        {
            // Arrange
            var options = CreateNewContextOptions();

            // Use a clean instance of the context to run the test
            await using (var context = new IdentityDbContext(options))
            {
                var repository = new UsersRepository(context);
                var userService = new UsersService(repository);
                var userDto = new UserDto { Name = "Test User", Email = "test@example.com" };

                // Act
                await userService.AddAsync(userDto);
            }

            // Assert
            await using (var context = new IdentityDbContext(options))
            {
                var repository = new UsersRepository(context);
                var users = await repository.ListAsync();

                Assert.Single(users);
                Assert.Equal("Test User", users[0].Name);
            }
        }

        [Fact]
        public async Task UsersService_MakeAdminAsync_ChangeUser_WhenPasswordIsCorrect()
        {
            // Arrange
            var options = CreateNewContextOptions();

            // Use a clean instance of the context to run the test
            await using (var context = new IdentityDbContext(options))
            {
                var repository = new UsersRepository(context);
                var userService = new UsersService(repository);
                var userDto = new UserDto { Name = "Test User", Email = "test@example.com" };

                // Add user to database
                await userService.AddAsync(userDto);

                // Act
                await userService.MakeAdminAsync(userDto.Email, "admin");
            }

            // Assert
            await using (var context = new IdentityDbContext(options))
            {
                var dbUser = await context.Users.FirstOrDefaultAsync(u => u.Email == "test@example.com");
                Assert.NotNull(dbUser);
                Assert.True(dbUser.IsAdmin);
            }
        }

        [Fact]
        public async Task UsersService_MakeAdminAsync_DoesNotChangeUser_WhenPasswordIsIncorrect()
        {
            // Arrange
            var options = CreateNewContextOptions();

            // Use a clean instance of the context to run the test
            await using (var context = new IdentityDbContext(options))
            {
                var repository = new UsersRepository(context);
                var userService = new UsersService(repository);
                var userDto = new UserDto { Name = "Test User", Email = "test@example.com" };

                // Add user to database
                await userService.AddAsync(userDto);

                // Act
                await userService.MakeAdminAsync(userDto.Email, "wrongpassword");
            }

            // Assert
            await using (var context = new IdentityDbContext(options))
            {
                var dbUser = await context.Users.FirstOrDefaultAsync(u => u.Email == "test@example.com");
                Assert.NotNull(dbUser);
                Assert.False(dbUser.IsAdmin);
            }
        }

        [Fact]
        public async Task UsersService_FindOrCreateUser_ReturnsExistingUser_WhenUserExists()
        {
            // Arrange
            var options = CreateNewContextOptions();

            // Use a clean instance of the context to run the test
            await using (var context = new IdentityDbContext(options))
            {
                var repository = new UsersRepository(context);
                var userService = new UsersService(repository);
                var userDto = new UserDto { Name = "Test User", Email = "test@example.com" };

                // Add user to database
                await userService.AddAsync(userDto);
            }

            // Act
            var existingUser = new UserDto { Name = "Test User", Email = "test@example.com" };
            await using (var context = new IdentityDbContext(options))
            {
                var repository = new UsersRepository(context);
                var userService = new UsersService(repository);
                var result = await userService.FindOrCreateUser(existingUser.Email, existingUser.Name);

                // Assert
                Assert.Equal(existingUser.Email, result.Email);
                Assert.Equal(existingUser.Name, result.Name);
            }
        }

        [Fact]
        public async Task UsersService_FindOrCreateUser_CreateNewUser_WhenUserDoesNotExist()
        {
            // Arrange
            var options = CreateNewContextOptions();

            // Use a clean instance of the context to run the test
            await using (var context = new IdentityDbContext(options))
            {
                var repository = new UsersRepository(context);
                var userService = new UsersService(repository);
            }

            // Act
            var newUser = new UserDto { Name = "New User", Email = "new@example.com" };
            await using (var context = new IdentityDbContext(options))
            {
                var repository = new UsersRepository(context);
                var userService = new UsersService(repository);
                var result = await userService.FindOrCreateUser(newUser.Email, newUser.Name);

                // Assert
                Assert.Equal(newUser.Email, result.Email);
                Assert.Equal(newUser.Name, result.Name);
            }
        }

        [Fact]
        public async Task UsersService_ListAsync_ReturnListOfUserDtos()
        {
            // Arrange
            var options = CreateNewContextOptions();

            // Use a clean instance of the context to run the test
            await using (var context = new IdentityDbContext(options))
            {
                var repository = new UsersRepository(context);
                var userService = new UsersService(repository);
                var userDto = new UserDto { Name = "Test User 1", Email = "test1@example.com" };

                // Add user to database
                await userService.AddAsync(userDto);
            }

            // Act
            await using (var context = new IdentityDbContext(options))
            {
                var repository = new UsersRepository(context);
                var userService = new UsersService(repository);
                var result = await userService.ListAsync();

                // Assert
                Assert.Single(result);
                Assert.Equal("Test User 1", result[0].Name);
            }
        }
        
        [Fact]
        public async Task UsersRepository_FindByEmailAsync_ReturnUser_WhenEmailExists()
        {
            // Arrange
            var options = CreateNewContextOptions();

            // Use a clean instance of the context to run the test
            await using (var context = new IdentityDbContext(options))
            {
                var repository = new UsersRepository(context);
                var user = new User { Name = "Test User", Email = "test@example.com" };

                // Add user to database
                await repository.AddAsync(user);
            }

            // Act
            await using (var context = new IdentityDbContext(options))
            {
                var repository = new UsersRepository(context);
                var result = await repository.FindByEmailAsync("test@example.com");

                // Assert
                Assert.NotNull(result);
                Assert.Equal("Test User", result.Name);
                Assert.Equal("test@example.com", result.Email);
            }
        }

        [Fact]
        public async Task UsersRepository_FindByEmailAsync_ReturnNull_WhenEmailDoesNotExist()
        {
            // Arrange
            var options = CreateNewContextOptions();

            // Use a clean instance of the context to run the test
            await using (var context = new IdentityDbContext(options))
            {
                var repository = new UsersRepository(context);
            }

            // Act
            await using (var context = new IdentityDbContext(options))
            {
                var repository = new UsersRepository(context);
                var result = await repository.FindByEmailAsync("nonexistent@example.com");

                // Assert
                Assert.Null(result);
            }
        }
    }
}

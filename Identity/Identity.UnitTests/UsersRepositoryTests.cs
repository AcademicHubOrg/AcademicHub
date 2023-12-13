using Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.UnitTests;

public class UsersRepositoryTests
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
    public async Task AddAsync_AddsUserToDatabase()
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
    public async Task ChangeUserAsync_ChangeUsersIsAdminToTrue()
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
            await repository.ChangeUserRoleToAdminAsync(user.Email);
            
        }
        // Assert
        await using (var context = new IdentityDbContext(options))
        {
            Assert.Equal(1, await context.Users.CountAsync());
            var dbUser = await context.Users.FirstOrDefaultAsync(u => u.Email == "test@example.com");
            Assert.NotNull(dbUser);
            Assert.Equal("Test User", dbUser.Name);
            Assert.True(dbUser.Role == Roles.Admin);
        }
    }
}

using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Identity.Core;
using Identity.Data;

namespace Identity.IntegrationTests
{
    public class IntegrationTest
    {
        [Fact]
        public async Task MultipleOperations_IntegrationTest()
        {
            // Arrange
            var dbContextOptions = CreateNewContextOptions();
            var repository = new UsersRepository(new IdentityDbContext(dbContextOptions));
            var userService = new UsersService(repository);

            // Create user DTOs for testing
            var userDto1 = new UserDto { Name = "Integration Test User 1", Email = "integration1@test.com" };
            var userDto2 = new UserDto { Name = "Integration Test User 2", Email = "integration2@test.com" };
            var userDto3 = new UserDto { Name = "Integration Test User 3", Email = "integration3@test.com" };

            // Act
            await userService.AddAsync(userDto1);
            await userService.AddAsync(userDto2);
            await userService.AddAsync(userDto3);
            await userService.MakeAdminAsync(userDto1.Email, "admin"); // Assuming "admin" is the correct password

            
            var userList = await userService.ListAsync();

            
            var resultUser = await userService.FindOrCreateUser(userDto3.Email, userDto3.Name);

            // Assert
            await using (var context = new IdentityDbContext(dbContextOptions))
            {
                
                var user1 = await context.Users.FirstOrDefaultAsync(u => u.Email == userDto1.Email);
                Assert.NotNull(user1);
                Assert.True(user1.IsAdmin);
                
                Assert.Equal(3, userList.Count);
                
                var user3 = await context.Users.FirstOrDefaultAsync(u => u.Email == userDto3.Email);
                Assert.NotNull(user3);
                Assert.Equal(userDto3.Email, user3.Email);
                Assert.Equal(userDto3.Name, user3.Name);
                
                Assert.Equal(userDto3.Email, resultUser.Email);
                Assert.Equal(userDto3.Name, resultUser.Name);
            }
        }
        
        private DbContextOptions<IdentityDbContext> CreateNewContextOptions()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<IdentityDbContext>();
            builder.UseInMemoryDatabase("IntegrationTestDataBase")
                .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }
    }
}
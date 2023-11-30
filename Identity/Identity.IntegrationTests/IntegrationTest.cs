using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Identity.Core;
using Identity.Data;

namespace Identity.IntegrationTests
{
    public class IntegrationTest
    {
        public static IEnumerable<object[]> UserScenarios()
        {
            yield return new object[]
            {
                new UserDto { Name = "Integration Test User 1", Email = "integration1@test.com" }
            };
            yield return new object[]
            {
                new UserDto { Name = "Integration Test User 2", Email = "integration2@test.com" }
            };
            yield return new object[]
            {
                new UserDto { Name = "Integration Test User 3", Email = "integration3@test.com" }
            };
        }
        
        public static IEnumerable<object[]> UserScenarios2()
        {
            yield return new object[]
            {
                new UserDto { Name = "Integration Test User 4", Email = "integration4@test.com" },
                new UserDto { Name = "Integration Test User 5", Email = "integration5@test.com" }
            };
        }
        public static IEnumerable<object[]> UserScenarios3()
        {
            yield return new object[]
            {
                new UserDto { Name = "Integration Test User 6", Email = "integration6@test.com" },
                new UserDto { Name = "Integration Test User 7" } // No email provided intentionally
            };
        }

        [Theory]
        [MemberData(nameof(UserScenarios))]
        public async Task MultipleOperations_IntegrationTest(UserDto userDto)
        {
            // Arrange
            var dbContextOptions = CreateNewContextOptions();
            var repository = new UsersRepository(new IdentityDbContext(dbContextOptions));
            var userService = new UsersService(repository);

            // Act
            await userService.AddAsync(userDto);
            await userService.MakeAdminAsync(userDto.Email, "admin");
            
            var userList = await userService.ListAsync();
            
            var resultUser = await userService.FindOrCreateUser(userDto.Email, userDto.Name);

            // Assert
            using (var context = new IdentityDbContext(dbContextOptions))
            {
                // Check if the user is admin
                var user = await context.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email);
                Assert.NotNull(user);
                Assert.True(user.IsAdmin);

                // Check if all users are listed
                Assert.Single(userList);

                // Check if the user was created or found
                Assert.NotNull(resultUser);
                Assert.Equal(userDto.Email, resultUser.Email);
                Assert.Equal(userDto.Name, resultUser.Name);
            }
        }

        [Theory]
        [MemberData(nameof(UserScenarios2))]
        public async Task MultipleOperations_IntegrationTest2(UserDto userDto1, UserDto userDto2)
        {
            // Arrange
            var dbContextOptions = CreateNewContextOptions();
            var repository = new UsersRepository(new IdentityDbContext(dbContextOptions));
            var userService = new UsersService(repository);

            // Act
            await userService.AddAsync(userDto1);
            await userService.AddAsync(userDto2);
            
            await userService.MakeAdminAsync(userDto1.Email, "wrongpassword");
            
            var userList = await userService.ListAsync();
            
            await userService.MakeAdminAsync(userDto2.Email, "admin");

            // Assert
            using (var context = new IdentityDbContext(dbContextOptions))
            {
                // First user should not be admin due to incorrect password
                var user1 = await context.Users.FirstOrDefaultAsync(u => u.Email == userDto1.Email);
                Assert.NotNull(user1);
                Assert.False(user1.IsAdmin);

                // Check if all users are listed
                Assert.Equal(2, userList.Count);

                // Second user should be admin now
                var user2 = await context.Users.FirstOrDefaultAsync(u => u.Email == userDto2.Email);
                Assert.NotNull(user2);
                Assert.True(user2.IsAdmin);
            }
        }
        
        [Theory]
        [MemberData(nameof(UserScenarios3))]
        public async Task MultipleOperations_IntegrationTest3(UserDto userDto1, UserDto userDto2)
        {
            // Arrange
            var dbContextOptions = CreateNewContextOptions();
            var repository = new UsersRepository(new IdentityDbContext(dbContextOptions));
            var userService = new UsersService(repository);

            // Act
            await userService.AddAsync(userDto1);

            // Assert that adding a user without an email triggers a DbUpdateException
            await Assert.ThrowsAsync<DbUpdateException>(async () => await userService.AddAsync(userDto2));
            
            var userList = await userService.ListAsync();

            // Assert
            using (var context = new IdentityDbContext(dbContextOptions))
            {
                // Check if all users are listed (only the first one should be added successfully)
                Assert.Single(userList);

                // Check if the first user is listed
                var user1 = await context.Users.FirstOrDefaultAsync(u => u.Email == userDto1.Email);
                Assert.NotNull(user1);
                Assert.Equal(userDto1.Name, user1.Name);

                // Ensure that the second user is not added due to the exception
                var user2 = await context.Users.FirstOrDefaultAsync(u => u.Name == userDto2.Name);
                Assert.Null(user2);
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
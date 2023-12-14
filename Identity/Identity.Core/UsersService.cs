﻿using Identity.Data;

namespace Identity.Core;

public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
}

public class UsersService
{
    private readonly IUsersRepository _repository;

    public UsersService(IUsersRepository repository)
    {
        _repository = repository;
    }

    public async Task AddAsync(UserDto user)
    {
        await _repository.AddAsync(new User()
        {
            Email = user.Email,
            Name = user.Name,
        });
    }

    public async Task<List<UserDto>> ListAsync()
    {
        var result = new List<UserDto>();
        var dbUsers = await _repository.ListAsync();
        var orderedUsers = dbUsers.OrderBy(u => u.Id).ToList(); // This will order by ID

        foreach (var user in orderedUsers)
        {
            result.Add(new UserDto()
            {
                Email = user.Email,
                Name = user.Name,
                Id = user.Id,
            });
        }

        return result;
    }

    public async Task<UserDto> FindOrCreateUser(string email, string name)
    {
        // Check if user exists
        var user = await _repository.FindByEmailAsync(email);
        if (user == null)
        {
            // Create a new user if doesn't exist
            user = new User {Email = email, Name = name, Role = Roles.User};
            await AddAsync(new UserDto {Email = email, Name = name});
        }

        return new UserDto {Id = user.Id, Email = user.Email, Name = user.Name};
    }

    public async Task MakeAdminAsync(string email, string password)
    {
        if (password != "admin")
        {
            return;
        }

        await _repository.ChangeUserRoleToAdminAsync(email);
    }

    public async Task<IEnumerable<string>> GetUserRolesAsync(string email)
    {
        var role = await _repository.GetUserRole(email);
        
        return new List<string> { role.ToString() };
    }

}
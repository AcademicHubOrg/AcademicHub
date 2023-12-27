using Identity.Data;
namespace Identity.Core;

public class UserShowDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
}

public class UserAddDto
{
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

    public async Task AddAsync(UserAddDto user)
    {
        await _repository.AddAsync(new User()
        {
            Email = user.Email,
            Name = user.Name,
        });
    }

    public async Task<List<UserShowDto>> ListAsync()
    {
        var result = new List<UserShowDto>();
        var dbUsers = await _repository.ListAsync();
        var orderedUsers = dbUsers.OrderBy(u => u.Id).ToList(); // This will order by ID

        foreach (var user in orderedUsers)
        {
            result.Add(new UserShowDto()
            {
                Email = user.Email,
                Name = user.Name,
                Id = user.Id,
            });
        }

        return result;
    }

    public async Task<UserAddDto> FindOrCreateUser(UserAddDto userToAdd)
    {
        // Check if user exists
        var user = await _repository.FindByEmailAsync(userToAdd.Email);
        if (user == null)
        {
            await AddAsync(userToAdd);
        }
        return userToAdd;
    }

    public async Task MakeAdminAsync(string email, string password)
    {
        if (password != "admin")
        {
            return;
        }

        await _repository.ChangeUserAsync(email);
    }

    public async Task<UserShowDto> GetByEmailAsync(string email)
    {
        var user = await _repository.FindByEmailAsync(email);
        return new UserShowDto()
        {
            Email = user.Email,
            Name = user.Name,
            Id = user.Id,
        };
    }
}
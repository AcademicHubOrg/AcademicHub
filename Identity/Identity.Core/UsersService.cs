using Identity.Data;

namespace Identity.Core;

public class UserDto
{
  public int Id { get; set; }
  public string Name { get; set; } = null!;
  public string Email { get; set; } = null!;
}

public class UsersService
{
  private readonly UsersRepository _repository;
  public UsersService()
  {
    _repository = new UsersRepository();
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
    foreach (var user in dbUsers)
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
}
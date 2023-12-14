namespace Identity.Data;

public interface IUsersRepository
{
    Task AddAsync(User user);
    Task<List<User>> ListAsync();

    Task<User> FindByEmailAsync(string email);

    Task ChangeUserAsync(string email);
}
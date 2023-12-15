namespace Identity.Data;

public interface IUsersRepository
{
    Task AddAsync(User user);
    Task<List<User>> ListAsync(int start_index = 0, int how_many = 10);

    Task<User> FindByEmailAsync(string email);

    Task ChangeUserAsync(string email);
}
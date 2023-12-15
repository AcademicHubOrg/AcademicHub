namespace Identity.Data;

public interface IUsersRepository
{
    Task AddAsync(User user);
    Task<List<User>> ListAsync(int start_index, int how_many);

    Task<User> FindByEmailAsync(string email);

    Task ChangeUserAsync(string email);
}
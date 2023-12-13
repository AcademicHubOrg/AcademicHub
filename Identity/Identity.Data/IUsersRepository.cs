namespace Identity.Data;

public interface IUsersRepository
{
    Task AddAsync(User user);
    Task<List<User>> ListAsync();

    Task<User> FindByEmailAsync(string email);

    Task ChangeUserRoleToAdminAsync(string email);

    Task<Roles> GetUserRole(string email);
}
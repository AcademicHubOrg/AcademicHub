using Microsoft.EntityFrameworkCore;

namespace Identity.Data;

public class UsersRepository
{
  private readonly IdentityDbContext _context;

  public UsersRepository()
  {
    _context = new IdentityDbContext();
  }
  
  public async Task AddAsync(User user)
  {
    _context.Add(user);
    await _context.SaveChangesAsync();
  }

  public async Task<List<User>> ListAsync()
  {
    return await _context.Users.ToListAsync();
  }
  
  public async Task<User> FindByEmailAsync(string email)
  {
    return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
  }
}
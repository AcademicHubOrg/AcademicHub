using Microsoft.EntityFrameworkCore;

namespace Identity.Data;

public class IdentityDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;

    public IdentityDbContext(DbContextOptions options)
        : base(options)
    {
    }
}

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;

    public bool IsAdmin { get; set; }
}
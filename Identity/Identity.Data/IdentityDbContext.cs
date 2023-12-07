using Microsoft.EntityFrameworkCore;

namespace Identity.Data;

public class IdentityDbContext : DbContext
{
  public DbSet<User> Users { get; set; } = null!;

  public IdentityDbContext()
  {
  }
  public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
  {
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseNpgsql("Server=postgres; Database=Identity;User Id=identityuser;Password=identityuser;Port=5432");
    base.OnConfiguring(optionsBuilder);
  }
}

public class User
{
  public int Id { get; set; }
  public string Name { get; set; } = null!;
  public string Email { get; set; } = null!;
  
  public bool IsAdmin { get; set; }
}

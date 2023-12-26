using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Identity.Data;

public class IdentityDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;

    public IdentityDbContext(DbContextOptions options)
        : base(options)
    {
    }
    
    // Design factory for migrations
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
    {
        public IdentityDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<IdentityDbContext>();
            optionsBuilder.UseNpgsql("Host=postgres; Database=Identity; User Id=identityuser; Password=identityuser; Port=5432");

            return new IdentityDbContext(optionsBuilder.Options);
        }
    }

}

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;

    public bool IsAdmin { get; set; }
}
using Microsoft.EntityFrameworkCore;

namespace Materials.Data;

public class MaterialsDbContext : DbContext
{
    public DbSet<MaterialData> Materials { get; set; } = null!;

    public MaterialsDbContext(DbContextOptions options)
        : base(options)
    {
    }
}
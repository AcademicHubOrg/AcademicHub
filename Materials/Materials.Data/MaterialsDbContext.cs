using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Materials.Data;

public class MaterialsDbContext : DbContext
{
    public DbSet<MaterialData> Materials { get; set; } = null!;
    public DbSet<EssentialMaterial> EssentialMaterials { get; set; } = null!;

    public MaterialsDbContext(DbContextOptions options)
        : base(options)
    {
    }
    // Design factory for migrations
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MaterialsDbContext>
    {
        public MaterialsDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MaterialsDbContext>();
            optionsBuilder.UseNpgsql("Host=postgres; Database=Identity; User Id=identityuser; Password=identityuser; Port=5432");

            return new MaterialsDbContext(optionsBuilder.Options);
        }
    }
}
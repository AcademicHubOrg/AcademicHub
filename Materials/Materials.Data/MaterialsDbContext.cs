using Microsoft.EntityFrameworkCore;

namespace Materials.Data;

public class MaterialsDbContext : DbContext
{
    public DbSet<MaterialData> Materials { get; set; }
    public DbSet<EssentialMaterial> EssentialMaterials { get; set; }
    public MaterialsDbContext()
    {
		
    }
  
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Database=Materials;User Id=materialsuser;Password=materialsuser;Port=5439;");
        base.OnConfiguring(optionsBuilder);
    }
}
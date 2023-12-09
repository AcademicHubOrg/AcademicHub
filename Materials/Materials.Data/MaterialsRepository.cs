namespace Materials.Data;

using Microsoft.EntityFrameworkCore;

public class MaterialsRepository : IMaterialsRepository
{
    private readonly MaterialsDbContext _context;

    public MaterialsRepository(MaterialsDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(MaterialData materialData)
    {
        _context.Add(materialData);
        await _context.SaveChangesAsync();
    }

    public async Task<List<MaterialData>> ListAsync()
    {
        return await _context.Materials.ToListAsync();
    }
}
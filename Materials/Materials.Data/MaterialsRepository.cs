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

    public async Task AddEssentialAsync(EssentialMaterial essentialData)
    {
        _context.Add(essentialData);
        await _context.SaveChangesAsync();
    }
    public async Task<List<MaterialData>> ListAsync(int start_index =0, int how_many=10)
    {
        if(start_index >= 0 && how_many > start_index)
        {
            return await _context.Materials
                .Skip(start_index)
                .Take(how_many)
                .ToListAsync();
        }
        else
        {
            return await _context.Materials
                .Skip(0)
                .Take(10)
                .ToListAsync();
        }
    }

    public async Task<List<EssentialMaterial>> ListEssentialsAsync()
    {
        return await _context.EssentialMaterials.ToListAsync();
    }
}
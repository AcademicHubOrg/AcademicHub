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

    public async Task<List<MaterialData>> ListAsync()
    {
        return await _context.Materials.ToListAsync();
    }

    public async Task<List<EssentialMaterial>> ListEssentialsAsync()
    {
        return await _context.EssentialMaterials.ToListAsync();
    }
    
    public async Task<MaterialData> GetAsync(int materialId)
    {
        return await _context.Materials.FindAsync(materialId);
    }

    public async Task DeleteAsync(MaterialData material)
    {
        _context.Materials.Remove(material);
        await _context.SaveChangesAsync();
    }

    public async Task<EssentialMaterial> GetEssentialAsync(int essentialId)
    {
        return await _context.EssentialMaterials.FindAsync(essentialId);
    }

    public async Task DeleteEssentialAsync(EssentialMaterial essentialMaterial)
    {
        _context.EssentialMaterials.Remove(essentialMaterial);
        await _context.SaveChangesAsync();
    }
    
    
    public async Task DeleteByCourseAsync(int courseId)
    {
        var materialsToDelete = await _context.Materials
            .Where(material => material.CourseId == courseId)
            .ToListAsync();

        _context.Materials.RemoveRange(materialsToDelete);
        await _context.SaveChangesAsync();
    }      
    
    public async Task DeleteByTemplateAsync(int templateId)
    {
        var materialsToDelete = await _context.Materials
            .Where(material => material.CourseId == templateId)
            .ToListAsync();

        _context.Materials.RemoveRange(materialsToDelete);
    
        var essentialsToDelete = await _context.EssentialMaterials
            .Where(essential => essential.TemplateId == templateId)
            .ToListAsync();

        _context.EssentialMaterials.RemoveRange(essentialsToDelete);

        await _context.SaveChangesAsync();
    } 
}
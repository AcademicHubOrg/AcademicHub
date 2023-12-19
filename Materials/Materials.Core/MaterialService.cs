using CustomExceptions;
using Materials.Data;

namespace Materials.Core;

public class MaterialDataDtoAdd
{
    public string Name { get; set; } = null!;
    public string DataText { get; set; } = null!;
    public string CourseId { get; set; } = null!;
}

public class MaterialDataDtoShow
{
    public string Name { get; set; } = null!;
    public string Id { get; set; } = null!;
    public string DataText { get; set; } = null!;
    public string CourseId { get; set; } = null!;
}

//this class may be extended to store all types of materials  
//some attributes may be missing depending on data type
public class MaterialShowData
{
    public string Name { get; set; } = null!;
    public string DataText { get; set; } = null!;
}

public class MaterialService
{
    private readonly IMaterialsRepository _repository;

    public MaterialService(IMaterialsRepository repository)
    {
        _repository = repository;
    }

    public async Task AddAsync(MaterialDataDtoAdd material)
    {
        var courseId = Convert.ToInt32(material.CourseId);
        var dbMaterials = await _repository.ListAsync();
        if (dbMaterials.Any(m => m.MaterialName == material.Name && m.CourseId == courseId))
        {
            throw new ConflictException(
                $"The course with id: '{courseId}' contains a material with the name '{material.Name}'");
        }

        await _repository.AddAsync(new MaterialData()
        {
            MaterialName = material.Name,
            DataText = material.DataText,
            CourseId = Convert.ToInt32(material.CourseId)
        });
    }

    public async Task AddEssentialAsync(MaterialDataDtoAdd material)
    {
        var templateId = Convert.ToInt32(material.CourseId);
        var dbEssentials = await _repository.ListEssentialsAsync();
        if (dbEssentials.Any(m => m.MaterialName == material.Name && m.TemplateId == templateId))
        {
            throw new ConflictException($"The template with id: '{templateId}' contains a material with the name '{material.Name}'");
        }

        await _repository.AddEssentialAsync(new EssentialMaterial()
        {
            MaterialName = material.Name,
            DataText = material.DataText,
            TemplateId = Convert.ToInt32(material.CourseId)
        });
    }

    public async Task<List<MaterialDataDtoShow>> ListAsync()
    {
        var result = new List<MaterialDataDtoShow>();
        var dbMaterials = await _repository.ListAsync();
        foreach (var materialData in dbMaterials)
        {
            result.Add(new MaterialDataDtoShow()
            {
                Name = materialData.MaterialName,
                Id = materialData.Id.ToString(),
                DataText = materialData.DataText,
                CourseId = materialData.CourseId.ToString()
            });
        }

        return result;
    }

    public async Task<List<MaterialDataDtoShow>> ListEssentialsAsync()
    {
        var result = new List<MaterialDataDtoShow>();
        var dbEssentials = await _repository.ListEssentialsAsync();
        foreach (var essentialData in dbEssentials)
        {
            result.Add(new MaterialDataDtoShow()
            {
                Name = essentialData.MaterialName,
                Id = essentialData.Id.ToString(),
                DataText = essentialData.DataText,
                CourseId = essentialData.TemplateId.ToString()
            });
        }
        return result;
    }

    
    public async Task<List<MaterialShowData>> ListByCourseIdAsync(int courseId)
    {
        var result = new List<MaterialShowData>();
        var dbMaterials = await _repository.ListAsync();
        foreach (var materialData in dbMaterials)
        {
            if (courseId == materialData.CourseId)
            {
                result.Add(new MaterialShowData()
                {
                    Name = materialData.MaterialName,
                    DataText = materialData.DataText,
                });
            }
        }
        if (result.Count == 0)
        {
            throw new NotFoundException($"Material with course ID: '{courseId}'");
        }
        return result;
    }

    public async Task<List<MaterialShowData>> ListByTemplateIdAsync(int templateId)
    {
        var result = new List<MaterialShowData>();
        var dbEssentials = await _repository.ListEssentialsAsync();
        foreach (var essentialData in dbEssentials)
        {
            if (templateId == essentialData.TemplateId)
            {
                result.Add(new MaterialShowData()
                {
                    Name = essentialData.MaterialName,
                    DataText = essentialData.DataText,
                });
            }
        }
        if (result.Count == 0)
        {
            throw new NotFoundException($"Material with template ID: '{templateId}'");
        }
        return result;
    }
    
    public async Task<List<MaterialShowData>> ListByIdAsync(int materialId)
    {
        var result = new List<MaterialShowData>();
        var dbMaterials = await _repository.ListAsync();
        foreach (var materialData in dbMaterials)
        {
            if (materialId == materialData.Id)
            {
                result.Add(new MaterialShowData()
                {
                    Name = materialData.MaterialName,
                    DataText = materialData.DataText,
                });
            }
        }
        if (result.Count == 0)
        {
            throw new NotFoundException($"Material with ID: '{materialId}'");
        }
        return result;
    }

    public async Task<List<MaterialShowData>> ListEssentialsByIdAsync(int materialId)
    {
        var result = new List<MaterialShowData>();
        var dbEssentials = await _repository.ListEssentialsAsync();
        foreach (var essentialData in dbEssentials)
        {
            if (materialId == essentialData.Id)
            {
                result.Add(new MaterialShowData()
                {
                    Name = essentialData.MaterialName,
                    DataText = essentialData.DataText,
                });
            }
        }
        if (result.Count == 0)
        {
            throw new NotFoundException($"Material with ID: '{materialId}'");
        }
        return result;
    }
    
    public async Task DeleteMaterialAsync(int materialId)
    {
        var material = await _repository.GetAsync(materialId);

        if (material == null)
        {
            throw new NotFoundException($"Material with ID: '{materialId}' not found");
        }

        await _repository.DeleteAsync(material);
    }

    public async Task DeleteEssentialMaterialAsync(int essentialId)
    {
        var essentialMaterial = await _repository.GetEssentialAsync(essentialId);

        if (essentialMaterial == null)
        {
            throw new NotFoundException($"Essential material with ID: '{essentialId}' not found");
        }

        await _repository.DeleteEssentialAsync(essentialMaterial);
    }
    
}
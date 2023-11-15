using Materials.Data;

namespace Materials.Core;

using System.Security.Cryptography;



public class MaterialDataDto
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
    public string Name { get; set; }
    public string DataText { get; set; }
}

public class MaterialService
{
    private readonly MaterialsRepository _repository;
    public MaterialService()
    {
        _repository = new MaterialsRepository();
    }
  
    public async Task AddAsync(MaterialDataDto material)
    {
        await _repository.AddAsync(new MaterialData()
        {
            MaterialName = material.Name,
            DataText = material.DataText,
            CourseId = Convert.ToInt32( material.CourseId)
        });
    }

    public async Task<List<MaterialDataDto>> ListAsync()
    {
        var result = new List<MaterialDataDto>();
        var dbMaterials = await _repository.ListAsync();
        foreach (var materialData in dbMaterials)
        {
            result.Add(new MaterialDataDto()
            {
                Name = materialData.MaterialName,
                Id = materialData.Id.ToString(),
                DataText = materialData.DataText,
                CourseId = materialData.CourseId.ToString()
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
        return result;
    }
    
    public async Task<List<MaterialShowData>> ListByIdAsync(int materialId)
    {
        var result = new List<MaterialShowData>();
        var dbMaterials = await _repository.ListAsync();
        foreach (var materialData in dbMaterials)
        {
            if (materialId == materialData.CourseId)
            {
                result.Add(new MaterialShowData()
                {
                    Name = materialData.MaterialName,
                    DataText = materialData.DataText,
                });
            }
        }
        return result;
    }
    
}
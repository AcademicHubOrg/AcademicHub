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
}
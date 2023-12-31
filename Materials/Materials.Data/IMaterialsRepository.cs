﻿namespace Materials.Data;

public interface IMaterialsRepository
{
    Task AddAsync(MaterialData materialData);
    Task<List<MaterialData>> ListAsync();
    Task AddEssentialAsync(EssentialMaterial essentialData);
    Task<List<EssentialMaterial>> ListEssentialsAsync();
    Task<MaterialData> GetAsync(int materialId);
    Task DeleteAsync(MaterialData material);
    Task<EssentialMaterial> GetEssentialAsync(int essentialId);
    Task DeleteEssentialAsync(EssentialMaterial essentialMaterial);
    Task DeleteByCourseAsync(int courseId);
    Task DeleteByTemplateAsync(int templateId);
    
        
}
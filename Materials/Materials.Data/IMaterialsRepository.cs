namespace Materials.Data;

public interface IMaterialsRepository
{
    Task AddAsync(MaterialData materialData);
    Task<List<MaterialData>> ListAsync();
    Task AddEssentialAsync(EssentialMaterial essentialData);
    Task<List<EssentialMaterial>> ListEssentialsAsync();
}
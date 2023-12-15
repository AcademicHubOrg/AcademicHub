namespace Materials.Data;

public interface IMaterialsRepository
{
    Task AddAsync(MaterialData materialData);
    Task<List<MaterialData>> ListAsync(int start_index = 0, int how_many = 10);
    Task AddEssentialAsync(EssentialMaterial essentialData);
    Task<List<EssentialMaterial>> ListEssentialsAsync();
}
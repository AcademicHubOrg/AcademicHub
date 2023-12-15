namespace Materials.Data;

public interface IMaterialsRepository
{
    Task AddAsync(MaterialData materialData);
    Task<List<MaterialData>> ListAsync(int start_index, int how_many);
    Task AddEssentialAsync(EssentialMaterial essentialData);
    Task<List<EssentialMaterial>> ListEssentialsAsync();
}
using CharacterDatabaseAPI.Models;

namespace CharacterDatabaseAPI.Services;

public interface ICollectionService<TDocumentModel>
{
    public Task<List<TDocumentModel>> GetAsync();
    public Task<TDocumentModel?> GetAsync(string id);
    public Task CreateAsync(TDocumentModel newDocument);
    public Task CreateMultipleAsync(IEnumerable<TDocumentModel> newDocuments);
    public Task UpdateAsync(string id, TDocumentModel updatedDocument);
    public Task RemoveAsync(string id);
}
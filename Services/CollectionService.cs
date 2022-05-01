using CharacterDatabaseAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CharacterDatabaseAPI.Services;

public abstract class CollectionService<TDocumentModel> : ICollectionService<TDocumentModel> where TDocumentModel : IDocumentModel
{
    private readonly IMongoCollection<TDocumentModel> _Collection;

    public CollectionService(string collectionName,
        IOptions<CharacterDatabaseSettings> CategoryValueDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            CategoryValueDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            CategoryValueDatabaseSettings.Value.DatabaseName);
                
        _Collection = mongoDatabase.GetCollection<TDocumentModel>(
            collectionName);
    }

    public async Task<List<TDocumentModel>> GetAsync() =>
        await _Collection.Find(_ => true).ToListAsync();

    public async Task<TDocumentModel?> GetAsync(string id) =>
        await _Collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(TDocumentModel newDocument) =>
        await _Collection.InsertOneAsync(newDocument);

    public async Task CreateMultipleAsync(IEnumerable<TDocumentModel> newDocuments) =>
        await _Collection.InsertManyAsync(newDocuments);

    public async Task UpdateAsync(string id, TDocumentModel updatedDocument) =>
        await _Collection.ReplaceOneAsync(x => x.Id == id, updatedDocument);

    public async Task RemoveAsync(string id) =>
        await _Collection.DeleteOneAsync(x => x.Id == id);
}
using CharacterDatabaseAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CharacterDatabaseAPI.Services;

public class CategoryValueService
{
    private readonly IMongoCollection<CategoryValue> _CategoryValueCollection;

    public CategoryValueService(string collectionPrefix, string categoryTypeName,
        IOptions<CharacterDatabaseSettings> CategoryValueDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            CategoryValueDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            CategoryValueDatabaseSettings.Value.DatabaseName);
        
        string collectionName = collectionPrefix + categoryTypeName;
        
        _CategoryValueCollection = mongoDatabase.GetCollection<CategoryValue>(
            collectionName);
    }

    public async Task<List<CategoryValue>> GetAsync() =>
        await _CategoryValueCollection.Find(_ => true).ToListAsync();

    public async Task<CategoryValue?> GetAsync(string id) =>
        await _CategoryValueCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(CategoryValue newValue) =>
        await _CategoryValueCollection.InsertOneAsync(newValue);

    public async Task CreateMultipleAsync(IEnumerable<CategoryValue> newValues) =>
        await _CategoryValueCollection.InsertManyAsync(newValues);

    public async Task UpdateAsync(string id, CategoryValue updatedValue) =>
        await _CategoryValueCollection.ReplaceOneAsync(x => x.Id == id, updatedValue);

    public async Task RemoveAsync(string id) =>
        await _CategoryValueCollection.DeleteOneAsync(x => x.Id == id);
}
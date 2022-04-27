using CharacterDatabaseAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CharacterDatabaseAPI.Services;

public class CharacterCollectionService
{
    private readonly IMongoCollection<CharacterCollection> _CharacterCollectionCollection;

    public CharacterCollectionService(
        IOptions<CharacterDatabaseSettings> characterDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            characterDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            characterDatabaseSettings.Value.DatabaseName);

        _CharacterCollectionCollection = mongoDatabase.GetCollection<CharacterCollection>(
            characterDatabaseSettings.Value.CharacterCollectionCollectionName);
    }

    public async Task<List<CharacterCollection>> GetAsync() =>
        await _CharacterCollectionCollection.Find(_ => true).ToListAsync();

    public async Task<CharacterCollection?> GetAsync(string id) =>
        await _CharacterCollectionCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(CharacterCollection newCollection) =>
        await _CharacterCollectionCollection.InsertOneAsync(newCollection);

    public async Task UpdateAsync(string id, CharacterCollection updatedCollection) =>
        await _CharacterCollectionCollection.ReplaceOneAsync(x => x.Id == id, updatedCollection);

    public async Task RemoveAsync(string id) =>
        await _CharacterCollectionCollection.DeleteOneAsync(x => x.Id == id);
}
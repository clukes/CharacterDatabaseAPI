using CharacterDatabaseAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CharacterDatabaseAPI.Services;

public class CharacterService
{
    private readonly IMongoCollection<Character> _CharacterCollection;

    public CharacterService(string collectionPrefix,
        IOptions<CharacterDatabaseSettings> characterDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            characterDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            characterDatabaseSettings.Value.DatabaseName);
        
        string collectionName = collectionPrefix + characterDatabaseSettings.Value.CharacterCollectionSuffix;
        
        _CharacterCollection = mongoDatabase.GetCollection<Character>(
            collectionName);
    }

    public async Task<List<Character>> GetAsync() =>
        await _CharacterCollection.Find(_ => true).ToListAsync();

    public async Task<Character?> GetAsync(string id) =>
        await _CharacterCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Character newCharacter) =>
        await _CharacterCollection.InsertOneAsync(newCharacter);

    public async Task CreateMultipleAsync(IEnumerable<Character> newCharacters) =>
        await _CharacterCollection.InsertManyAsync(newCharacters);

    public async Task UpdateAsync(string id, Character updatedCharacter) =>
        await _CharacterCollection.ReplaceOneAsync(x => x.Id == id, updatedCharacter);

    public async Task RemoveAsync(string id) =>
        await _CharacterCollection.DeleteOneAsync(x => x.Id == id);
}
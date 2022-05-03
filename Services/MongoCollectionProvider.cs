using CharacterDatabaseAPI.Models;
using Microsoft.Extensions.Options;

namespace CharacterDatabaseAPI.Services;

public class MongoCollectionProvider
{
  // private readonly IMongoDatabase _Database;

  // public MongoCollectionProvider(IOptions<CharacterDatabaseSettings> CharacterDatabaseSettings) 
  // {
  //     var mongoClient = new MongoClient(
  //       CharacterDatabaseSettings.Value.ConnectionString);
      
  //     _Database = mongoClient.GetDatabase(
  //       CharacterDatabaseSettings.Value.DatabaseName);
  // }

  // public IMongoCollection<T> GetCollection<T>(string collectionName)
  // {
  //   return _Database.GetCollection<T>(collectionName);
  // }
}

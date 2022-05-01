using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CharacterDatabaseAPI.Models
{
    public class CharacterCollection
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Universe { get; set; }
        public string CollectionPrefix { get; set; }
        public IEnumerable<string> CategoryTypeNames { get; set; }
        public CharacterCollection(string universe, string collectionPrefix, IEnumerable<string> categoryTypeNames) 
        {
            Universe = universe;
            CollectionPrefix = collectionPrefix;
            CategoryTypeNames = categoryTypeNames;
        }
    }
}
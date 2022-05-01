using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;


namespace CharacterDatabaseAPI.Models
{
    public class CharacterCollection : IDocumentModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Id { get; set; }
        public string Universe { get; set; }
        public string CollectionPrefix { get; set; }
        public ICollection<string> CategoryTypeNames { get; set; }
        public CharacterCollection(string universe, string collectionPrefix, ICollection<string> categoryTypeNames) 
        {
            Universe = universe;
            CollectionPrefix = collectionPrefix;
            CategoryTypeNames = categoryTypeNames;
        }
    }
}
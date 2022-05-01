using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace CharacterDatabaseAPI.Models
{
    public class Character
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string[] Names { get; set; }
        public IDictionary<string, string> CategorySets { get; set; }

        public Character(string[] names, IDictionary<string, string>? categorySets = null) 
        {
            Names = names;
            CategorySets = categorySets ?? new Dictionary<string, string>();
        }

    }
}

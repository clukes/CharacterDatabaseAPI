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
        public IEnumerable<CategorySet> CategorySets { get; set; }

        public Character(string[] names, IEnumerable<CategorySet>? categorySets = null) 
        {
            Names = names;
            CategorySets = categorySets ?? new List<CategorySet>();
        }

    }
}

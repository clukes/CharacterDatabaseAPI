using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace CharacterDatabaseAPI.Models
{
    public class CategoryValue
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Value { get; set; }
        public IEnumerable<Character> Characters { get; set; }
        public CategoryValue(string value, IEnumerable<Character>? characters = null) 
        {
            Value = value;
            Characters = characters ?? new List<Character>();
        }
    }
}
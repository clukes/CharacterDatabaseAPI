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
        public IEnumerable<CategoryType> CategoryTypes { get; set; }
        public IEnumerable<Character> Characters { get; set; }
        public CharacterCollection(string universe, IEnumerable<CategoryType> categoryTypes, IEnumerable<Character> characters) 
        {
            Universe = universe;
            CategoryTypes = categoryTypes;
            Characters = characters;
        }
    }
}
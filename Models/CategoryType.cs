using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CharacterDatabaseAPI.Models
{
    public class CategoryType
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<CategoryValue> CategoryValues { get; set; }
        public CategoryType(string name, IEnumerable<CategoryValue>? categoryValues = null) 
        {
            Name = name;
            CategoryValues = categoryValues ?? new List<CategoryValue>();
        }

        public void AppendValue(CategoryValue categoryValue) {
            CategoryValues = CategoryValues.Append(categoryValue);
        }
    }
}
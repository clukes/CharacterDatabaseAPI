using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CharacterDatabaseAPI.Models
{
    public class CategorySet
    {
        public string CategoryName { get; set; }
        public string CategoryValue { get; set; }
        public CategorySet(string categoryName, string categoryValue) 
        {
            CategoryName = categoryName;
            CategoryValue = categoryValue;
        }
    }
}
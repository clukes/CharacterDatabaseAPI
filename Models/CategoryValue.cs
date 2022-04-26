namespace CharacterDatabaseAPI.Models
{
    public class CategoryValue
    {
        public long Id { get; set; }
        public CategoryType CategoryType { get; set; }
        public string Value { get; set; }

        public CategoryValue(CategoryType categoryType, string value) 
        {
            CategoryType = categoryType;
            Value = value;
        }
    }
}
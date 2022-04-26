namespace CharacterDatabaseAPI.Models
{
    public class NameCategoryValue
    {
        public long Id { get; set; }
        public Name Name { get; set; }
        public CategoryValue CategoryValue { get; set; }

        public NameCategoryValue(Name name, CategoryValue categoryValue) 
        {
            Name = name;
            CategoryValue = categoryValue;
        }

    }
}

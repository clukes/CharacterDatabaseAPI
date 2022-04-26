namespace CharacterDatabaseAPI.Models
{
    public class CategoryType
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public CategoryType(string name) 
        {
            Name = name;
        }
    }
}
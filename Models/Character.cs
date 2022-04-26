namespace CharacterDatabaseAPI.Models
{
    public class Character
    {
        public long Id { get; set; }
        public string[] Names { get; set; }
        public CategoryValue[] CategoryValues { get; set; }

        public Character(string[] names, CategoryValue[] categoryValues) 
        {
            Names = names;
            CategoryValues = categoryValues;
        }

    }
}

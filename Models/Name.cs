namespace CharacterDatabaseAPI.Models
{
    public class Name
    {
        public long Id { get; set; }
        public string[] Names { get; set; }

        public Name(string[] names) 
        {
            Names = names;
        }

    }
}
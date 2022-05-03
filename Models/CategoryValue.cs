using System.Text.Json.Serialization;
namespace CharacterDatabaseAPI.Models
{
    public class CategoryValue : IDocumentModel
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Id { get; set; }
        public string Value { get; set; }
        public ICollection<Character> Characters { get; set; }
        public CategoryValue(string value, ICollection<Character>? characters = null) 
        {
            Value = value;
            Characters = characters ?? new List<Character>();
        }

        public void AddCharacter(Character character) {
            Characters.Add(character);
        }
    }
}
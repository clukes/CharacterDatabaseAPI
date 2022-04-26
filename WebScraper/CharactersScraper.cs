using HtmlAgilityPack;
using System.Text.Json;
using CharacterDatabaseAPI.Models;

namespace CharacterDatabaseAPI.WebScraper
{
    public abstract class CharactersScraper
    {             
        private const string OutDir = "WebScraper/out/";
   
        public List<NameCategoryValue>? Characters { get; set; }
        
        public void WriteCharactersToJSONFile(string fileName = "Characters.json")
        {
            if(Characters == null) return;
            string jsonString = JsonSerializer.Serialize(Characters);
            Directory.CreateDirectory(OutDir);
            File.WriteAllText(OutDir+fileName, jsonString);
        }
        public abstract List<NameCategoryValue> RetrieveCharacters();
    }
}
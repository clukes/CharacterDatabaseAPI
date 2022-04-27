using HtmlAgilityPack;
using System.Text.Json;
using CharacterDatabaseAPI.Models;

namespace CharacterDatabaseAPI.WebScraper
{
    public abstract class CharactersScraper
    {   
        private const string DefaultFileName = "Characters.json";
        protected const string OutDir = "WebScraper/out/";
        public CharacterCollection? Characters { get; set; }
        
        public CharactersScraper(CharacterCollection? characters = null) 
        {
            Characters = characters;
        }
        public virtual void WriteCharactersToJSONFile(string fileName = DefaultFileName)
        {
            if(Characters == null) return;
            string jsonString = JsonSerializer.Serialize(Characters);
            Directory.CreateDirectory(OutDir);
            File.WriteAllText(OutDir+fileName, jsonString);
        }

        public virtual CharacterCollection? ReadCharactersFromJSONFile(string fileName = DefaultFileName)
        {
            string jsonString = File.ReadAllText(OutDir+fileName);
            return JsonSerializer.Deserialize<CharacterCollection>(jsonString);
        }

        public abstract CharacterCollection? RetrieveCharacters();
    }
}
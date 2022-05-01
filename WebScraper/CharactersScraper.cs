using HtmlAgilityPack;
using System.Text.Json;
using CharacterDatabaseAPI.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CharacterDatabaseAPI.WebScraper
{
    public abstract class CharactersScraper
    {   
        private readonly Dictionary<string, string> fileNames = new Dictionary<string, string>() 
        {
            {"CharacterCollection", "CharacterCollection.json"},
            {"Characters", "Characters.json"},
            {"CategoryValues", "CategoryValues.json"},
        };

        protected string DefaultDirName = "Universe/";
        protected const string OutDir = "WebScraper/out/";
        public CharacterCollection? CharacterCollection { get; set; }
        public ICollection<Character> Characters { get; set; }
        public ICollection<CategoryValue> CategoryValues { get; set; }

        public CharactersScraper(CharacterCollection? characterCollection = null, ICollection<Character>? characters = null, ICollection<CategoryValue>? categoryValues = null) 
        {
            CharacterCollection = characterCollection;
            Characters = characters ?? new List<Character>();
            CategoryValues = categoryValues ?? new List<CategoryValue>();
        }
        public void WriteDataToJSONFile(string? dirName = null)
        {
            dirName ??= DefaultDirName;
            WriteToJSONFile(CharacterCollection, dirName+fileNames["CharacterCollection"]);
            WriteToJSONFile(Characters, dirName+fileNames["Characters"]);
            WriteToJSONFile(CategoryValues, dirName+fileNames["CategoryValues"]);
        }

        public void ReadDataFromJSONFile(string? dirName = null)
        {
            dirName ??= DefaultDirName;
            CharacterCollection = ReadFromJSONFile<CharacterCollection>(dirName+fileNames["CharacterCollection"]) ?? CharacterCollection;
            Characters = ReadFromJSONFile<ICollection<Character>>(dirName+fileNames["Characters"]) ?? Characters;
            CategoryValues = ReadFromJSONFile<ICollection<CategoryValue>>(dirName+fileNames["CategoryValues"]) ?? CategoryValues;
        }

        public abstract ICollection<Character>? RetrieveCharacters();

        private T? ReadFromJSONFile<T>(string filePath)
        {
            string jsonString = File.ReadAllText(OutDir+filePath);
            return JsonSerializer.Deserialize<T>(jsonString);
        }

        private void WriteToJSONFile(Object? obj, string filePath) {
            string jsonString = JsonSerializer.Serialize(obj);
            Directory.CreateDirectory(OutDir);
            File.WriteAllText(OutDir+filePath, jsonString);
        }
    }
}
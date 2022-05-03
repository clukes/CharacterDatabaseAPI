using HtmlAgilityPack;
using System.Text.Json;
using CharacterDatabaseAPI.Models;

namespace CharacterDatabaseAPI.WebScraper
{
    public abstract class CharactersScraper
    {   
        private const string CharacterCollectionName = "CharacterCollection";
        private const string CharactersName = "Characters";
        private const string CategoryCollectionsName = "CategoryCollections";

        protected string DefaultDirName = "Universe/";
        protected const string OutDir = "WebScraper/out/";
        public CharacterCollection? CharacterCollection { get; set; }
        public ICollection<Character> Characters { get; set; }
        public IDictionary<string, ICollection<CategoryValue>> CategoryCollections { get; set; }

        public CharactersScraper(CharacterCollection? characterCollection = null, ICollection<Character>? characters = null, IDictionary<string, ICollection<CategoryValue>>? categoryCollections = null) 
        {
            CharacterCollection = characterCollection;
            Characters = characters ?? new List<Character>();
            CategoryCollections = categoryCollections ?? new Dictionary<string, ICollection<CategoryValue>>();
        }
        public void WriteDataToJSONFile(string? dirName = null)
        {
            dirName ??= DefaultDirName;
            WriteToJSONFile(CharacterCollection, GetFilePath(dirName, CharacterCollectionName));
            WriteToJSONFile(Characters, GetFilePath(dirName, CharactersName));
            WriteToJSONFile(CategoryCollections, GetFilePath(dirName, CategoryCollectionsName));
        }

        public void ReadDataFromJSONFile(string? dirName = null)
        {
            dirName ??= DefaultDirName;
            CharacterCollection = ReadFromJSONFile<CharacterCollection>(GetFilePath(dirName, CharacterCollectionName)) ?? CharacterCollection;
            Characters = ReadFromJSONFile<ICollection<Character>>(GetFilePath(dirName, CharactersName)) ?? Characters;
            CategoryCollections = ReadFromJSONFile<IDictionary<string, ICollection<CategoryValue>>>(GetFilePath(dirName, CategoryCollectionsName)) ?? CategoryCollections;
        }

        public abstract ICollection<Character>? RetrieveCharacters();

        private T? ReadFromJSONFile<T>(string filePath)
        {
            string jsonString = File.ReadAllText(OutDir+filePath);
            return JsonSerializer.Deserialize<T>(jsonString);
        }

        private void WriteToJSONFile(Object? obj, string filePath) 
        {
            string jsonString = JsonSerializer.Serialize(obj);
            Directory.CreateDirectory(Path.GetDirectoryName(OutDir+filePath)!);
            File.WriteAllText(OutDir+filePath, jsonString);
        }

        private string GetFilePath(string dirName, string filename) 
        {
            if(dirName.Last() != '/') dirName += '/';
            return dirName+filename+".json";
        }
    }
}
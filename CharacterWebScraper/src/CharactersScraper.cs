using HtmlAgilityPack;
using System.Text.Json;
using CharacterDatabaseAPI.Models;

namespace CharacterWebScraper;
public abstract class CharactersScraper
{
    private const string CharacterCollectionName = "CharacterCollection";
    private const string CharactersName = "Characters";
    private const string CategoryCollectionsName = "CategoryCollections";

    protected string DefaultDirName = "Universe/";
    protected const string OutDir = "out/";
    public ICollection<Character> Characters { get; set; }

    public CharactersScraper(ICollection<Character>? characters = null)
    {
        Characters = characters ?? new List<Character>();
    }
    public void WriteDataToJSONFile(string? dirName = null)
    {
        dirName ??= DefaultDirName;
        WriteToJSONFile(Characters, GetFilePath(dirName, CharactersName));
    }

    public void ReadDataFromJSONFile(string? dirName = null)
    {
        dirName ??= DefaultDirName;
        Characters = ReadFromJSONFile<ICollection<Character>>(GetFilePath(dirName, CharactersName)) ?? Characters;
    }

    public abstract ICollection<Character>? RetrieveCharacters();

    private T? ReadFromJSONFile<T>(string filePath)
    {
        string jsonString = File.ReadAllText(OutDir + filePath);
        return JsonSerializer.Deserialize<T>(jsonString);
    }

    private void WriteToJSONFile(Object? obj, string filePath)
    {
        string jsonString = JsonSerializer.Serialize(obj);
        Directory.CreateDirectory(Path.GetDirectoryName(OutDir + filePath)!);
        File.WriteAllText(OutDir + filePath, jsonString);
    }

    private string GetFilePath(string dirName, string filename)
    {
        if (dirName.Last() != '/') dirName += '/';
        return dirName + filename + ".json";
    }
}

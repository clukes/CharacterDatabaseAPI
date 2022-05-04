using CharacterDatabaseAPI.Models;
using CharacterDatabaseAPI.Services;
using CharacterDatabaseAPI;

namespace CharacterWebScraper;
public class WebScraperProgram
{
    private static StarWarsCharactersScraper starWarsCharactersScraper = new StarWarsCharactersScraper();
    public static void ScraperRetrieve()
    {
        ICollection<Character>? characters = starWarsCharactersScraper.RetrieveCharacters();
        if (characters == null)
        {
            Console.WriteLine("No Star Wars characters found");
            return;
        }
        starWarsCharactersScraper.WriteDataToJSONFile();
        Console.WriteLine("Star Wars characters written");
    }

    private static void ScraperUpdateFromJSON()
    {
        starWarsCharactersScraper.ReadDataFromJSONFile();
        if (starWarsCharactersScraper.Characters == null)
        {
            Console.WriteLine("No Star Wars characters found");
            return;
        }
        Console.WriteLine("Star Wars characters updated");
    }

    private static void ScraperUpdateIfEmpty()
    {
        if (starWarsCharactersScraper.Characters.Any()) return;
        ScraperUpdateFromJSON();
    }


    public async static void ScraperDBSave(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        ICharacterService? characterService = scope.ServiceProvider.GetService<ICharacterService>();

        if (characterService == null)
        {
            Console.WriteLine("No characterService.");
            return;
        }

        ScraperUpdateIfEmpty();
        if (starWarsCharactersScraper.Characters == null)
        {
            Console.WriteLine("No Star Wars characters found.");
            return;
        }

        starWarsCharactersScraper.WriteDataToJSONFile();
        await characterService.SaveManyAsync(starWarsCharactersScraper.Characters);
        Console.WriteLine("Star Wars characters added to database.");
        scope.Dispose();
    }

    public static void Main(string[] args)
    {
        var builder = CharacterDatabaseAPIProgram.CreateBuilder(args);
        var app = CharacterDatabaseAPIProgram.BuildApp(builder);

        WebScraperProgram.ScraperRetrieve();
        WebScraperProgram.ScraperDBSave(app.Services);
    }
}
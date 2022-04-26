using System;
using CharacterDatabaseAPI.Models;

namespace CharacterDatabaseAPI.WebScraper
{
    public class WebScraperProgram
    {
        public static void ScraperMain()
        {
            StarWarsCharactersScraper starWarsScraper = new StarWarsCharactersScraper();
            List<NameCategoryValue> data = starWarsScraper.RetrieveCharacters();
            starWarsScraper.WriteCharactersToJSONFile();
            Console.WriteLine(string.Join("\n", data));
        }
    }
}
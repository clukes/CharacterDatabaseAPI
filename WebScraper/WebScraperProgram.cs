using System;
using NameGeneratorAPI.Models;

namespace NameGeneratorAPI.WebScraper
{
    public class WebScraperProgram
    {
        public static void ScraperMain()
        {
            StarWarsCharactersScraper starWarsScraper = new StarWarsCharactersScraper();
            List<StarWarsName> data = starWarsScraper.RetrieveCharacters();
            starWarsScraper.WriteCharactersToJSONFile();
            Console.WriteLine(string.Join("\n", data));
        }
    }
}
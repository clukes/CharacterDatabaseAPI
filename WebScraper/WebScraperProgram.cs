using System;
using CharacterDatabaseAPI.Models;
using CharacterDatabaseAPI.Services;

namespace CharacterDatabaseAPI.WebScraper
{
    public class WebScraperProgram
    {
        private static StarWarsCharactersScraper starWarsCharactersScraper = new StarWarsCharactersScraper();
        public static void ScraperRetrieve()
        {
            ICollection<Character>? characters = starWarsCharactersScraper.RetrieveCharacters();
            if(characters == null) 
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
            if(starWarsCharactersScraper.Characters == null) 
            {
                Console.WriteLine("No Star Wars characters found");
                return;
            }
            Console.WriteLine("Star Wars characters updated");
        }

        private static void ScraperUpdateIfNull()
        {
            if (starWarsCharactersScraper.Characters != null) return; 
            ScraperUpdateFromJSON();
        }


        public async static void ScraperDBSave(CharacterCollectionService characterCollectionService, CategoryValueService categoryValueService, CharacterService characterService)
        {
            ScraperUpdateIfNull();
            if(starWarsCharactersScraper.CharacterCollection == null || starWarsCharactersScraper.Characters == null) 
            {
                Console.WriteLine("No Star Wars characters found.");
                return;
            }
            await characterCollectionService.CreateAsync(starWarsCharactersScraper.CharacterCollection);
            
            await categoryValueService.CreateAsync(starWarsCharactersScraper.CategoryValues);
            await characterService.CreateMultipleAsync(starWarsCharactersScraper.Characters);
            Console.WriteLine("Star Wars characters added to database.");
        }

    }
}
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

        private static void ScraperUpdateIfEmpty()
        {
            if (starWarsCharactersScraper.Characters.Any()) return; 
            ScraperUpdateFromJSON();
        }


        public async static void ScraperDBSave(IServiceProvider services)
        {
            ScraperUpdateIfEmpty();
            if(starWarsCharactersScraper.CharacterCollection == null || starWarsCharactersScraper.Characters == null) 
            {
                Console.WriteLine("No Star Wars characters found.");
                return;
            }
            // var characterCollectionService = services.GetService<CollectionService<CharacterCollection>>()!;
            // var categoryCategoryService = services.GetService<CollectionService<CategoryValue>>()!;
            // var speciesCategoryService = services.GetService<CollectionService<CategoryValue>>()!;
            // var characterService = services.GetService<CollectionService<Character>>()!;

            // await characterCollectionService.CreateOrUpdateAsync(starWarsCharactersScraper.CharacterCollection);
            // await characterService.CreateOrUpdateManyAsync(starWarsCharactersScraper.Characters);
            // await categoryCategoryService.CreateOrUpdateManyAsync(starWarsCharactersScraper.CategoryCollections["Category"]);
            // await speciesCategoryService.CreateOrUpdateManyAsync(starWarsCharactersScraper.CategoryCollections["Species"]);
            Console.WriteLine("Star Wars characters added to database.");
            starWarsCharactersScraper.WriteDataToJSONFile();
        }

    }
}
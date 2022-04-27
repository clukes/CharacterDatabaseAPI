using System;
using CharacterDatabaseAPI.Models;
using CharacterDatabaseAPI.Services;

namespace CharacterDatabaseAPI.WebScraper
{
    public class WebScraperProgram
    {
        private static CharacterCollection? characterCollection;
        private static StarWarsCharactersScraper starWarsCharactersScraper = new StarWarsCharactersScraper();
        public static void ScraperRetrieve()
        {
            characterCollection = starWarsCharactersScraper.RetrieveCharacters();
            starWarsCharactersScraper.WriteCharactersToJSONFile();
            if (characterCollection == null) return;
            Console.WriteLine(string.Join("\n", characterCollection.Characters));
        }

        private static void ScraperUpdateFromJSON()
        {
            characterCollection = starWarsCharactersScraper.ReadCharactersFromJSONFile();
            if (characterCollection == null) return;
            Console.WriteLine(string.Join("\n", characterCollection.Characters));
        }

        private static void ScraperUpdateIfNull()
        {
            if (characterCollection != null) return; 
            ScraperUpdateFromJSON();
        }


        public async static void ScraperDBSave(CharacterCollectionService characterCollectionService)
        {
            ScraperUpdateIfNull();
            if (characterCollection == null) return;
            Console.WriteLine(string.Join("\n", characterCollection.Characters));
            await characterCollectionService.CreateAsync(characterCollection);
        }

    }
}
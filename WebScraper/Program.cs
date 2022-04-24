using System;
using NameGeneratorAPI.WebScraper;

// See https://aka.ms/new-console-template for more information
StarWarsCharactersScraper starWarsScraper = new StarWarsCharactersScraper();
List<StarWarsCharacter> data = starWarsScraper.RetrieveCharacters();
starWarsScraper.WriteCharactersToJSONFile();
Console.WriteLine(string.Join("\n", data));


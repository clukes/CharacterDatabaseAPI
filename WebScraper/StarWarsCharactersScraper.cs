using HtmlAgilityPack;
using System.Text.Json;
using NameGeneratorAPI.Models;

namespace NameGeneratorAPI.WebScraper
{
    public class StarWarsCharactersScraper
    {
        public const string WikiPageUrl = "List_of_Star_Wars_characters";
        private const string HeadlineClass = "mw-headline";
        private const string StopHeadlineText = "See also";
        private const string OutDir = "out/";
        public List<StarWarsName>? Characters { get; set; }
        
        
        public void WriteCharactersToJSONFile(string fileName = "StarWarsCharacters.json")
        {
            if(Characters == null) return;
            string jsonString = JsonSerializer.Serialize(Characters);
            File.WriteAllText(OutDir+fileName, jsonString);

        }
        public List<StarWarsName> RetrieveCharacters() {
            HtmlDocument document = HTMLRetriever.GetWikiDocument(WikiPageUrl);
            System.Console.WriteLine($"Processing: {WikiPageUrl}");

            HtmlNodeCollection speciesGroupingsNodes = document.DocumentNode.SelectNodes($"//h2/span[@class='{HeadlineClass}'][following::h2/span[.='{StopHeadlineText}']]");

            if(speciesGroupingsNodes == null || !speciesGroupingsNodes.Any()) return new List<StarWarsName>();

            IEnumerable<StarWarsName> characters = speciesGroupingsNodes.SelectMany(x => CreateCharactersFromSpeciesGrouping(x));
            Characters = characters.ToList();
            return Characters;
        }

        private static IEnumerable<StarWarsName> CreateCharactersFromSpeciesGrouping(HtmlNode speciesGroupingNode) {
            string speciesGroupingName = speciesGroupingNode.InnerText;
            System.Console.WriteLine($"SpeciesGrouping: {speciesGroupingName}");

            HtmlNodeCollection categoryNodes = speciesGroupingNode.SelectNodes($"//following::h3/span[@class='{HeadlineClass}'][preceding::h2[1]/span[.=\"{speciesGroupingName}\"]]");

            if(categoryNodes == null || !categoryNodes.Any()) return new List<StarWarsName>();

            IEnumerable<StarWarsName> characters = categoryNodes.SelectMany(x => CreateCharactersFromCategory(speciesGroupingName, x));
            return characters;
        }

        private static IEnumerable<StarWarsName> CreateCharactersFromCategory(string speciesGroupingName, HtmlNode categoryNode) {
            string categoryName = categoryNode.InnerText;
            System.Console.WriteLine($"Category: {categoryName}");

            string speciesName, characterSelector;

            if (speciesGroupingName == "Humans")
            {
                speciesName = "Human";
                characterSelector = $"//following::tr/td[1][preceding::h3[1]/span[.=\"{categoryName}\"]]";
                return CreateCharactersFromSpecies(speciesGroupingName, speciesName, characterSelector, categoryNode);
            }

           
            HtmlNodeCollection speciesNodes = categoryNode.SelectNodes($"//following::h4/span[@class='{HeadlineClass}'][preceding::h3[1]/span[.=\"{categoryName}\"]]");
            if(speciesNodes == null || !speciesNodes.Any()) return new List<StarWarsName>();
            return speciesNodes.SelectMany(x => {
                string speciesName = x.InnerText;
                string characterSelector = $"//following::tr/td[1][preceding::h4[1]/span[.=\"{speciesName}\"]]";
                return CreateCharactersFromSpecies(speciesGroupingName, speciesName, characterSelector, categoryNode);
            });
        }

        private static IEnumerable<StarWarsName> CreateCharactersFromSpecies(string speciesGroupingName, string speciesName, string characterSelector, HtmlNode categoryNode) 
        {
            System.Console.WriteLine($"Species: {speciesName}");
            string categoryName = categoryNode.InnerText;

            HtmlNodeCollection characterNodes = categoryNode.SelectNodes(characterSelector);

            if(characterNodes == null || !characterNodes.Any()) return new List<StarWarsName>();

            IEnumerable<StarWarsName> characters = characterNodes.Select(x => CreateCharacter(speciesName, speciesGroupingName, categoryName, x));
            return characters;

        }
        private static StarWarsName CreateCharacter(string species, string speciesGrouping, string category, HtmlNode characterNode) {
            string name = characterNode.InnerText.Trim();
            HtmlNode alternateNameNode = characterNode.SelectSingleNode(".//span[@style='font-size:85%;']");
            string? alternateName = alternateNameNode?.InnerText.Trim();
            if(alternateName != null) name = name.Replace(alternateName, "").Trim();
            System.Console.WriteLine($"Name: {name}, Alternate: {alternateName}");

            return new StarWarsName(
                name: name,
                species: species,
                speciesGrouping: speciesGrouping,
                category: category,
                alternateName: alternateName);
        }
    }
}
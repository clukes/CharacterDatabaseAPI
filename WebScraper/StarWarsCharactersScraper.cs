using HtmlAgilityPack;
using System.Text.Json;
using CharacterDatabaseAPI.Models;

namespace CharacterDatabaseAPI.WebScraper
{
    public class StarWarsCharactersScraper : CharactersScraper
    {
        public const string WikiPageUrl = "List_of_Star_Wars_characters";
        private const string HeadlineClass = "mw-headline";
        private const string StopHeadlineText = "See also";
        private const string OutDir = "out/";
        public new void WriteCharactersToJSONFile(string fileName = "StarWarsCharacters.json") => base.WriteCharactersToJSONFile(fileName);

        public CategoryType SpeciesType { get; set; }
        public CategoryType CategoryType { get; set; }

        
        public StarWarsCharactersScraper(CategoryType? speciesType = null, CategoryType? categoryType = null) {
            SpeciesType = speciesType ?? new CategoryType("Species");
            CategoryType = categoryType ?? new CategoryType("Category");
        }
        public override List<Character> RetrieveCharacters() {
            HtmlDocument document = HTMLRetriever.GetWikiDocument(WikiPageUrl);
            System.Console.WriteLine($"Processing: {WikiPageUrl}");

            HtmlNodeCollection speciesGroupingsNodes = document.DocumentNode.SelectNodes($"//h2/span[@class='{HeadlineClass}'][following::h2/span[.='{StopHeadlineText}']]");

            if(speciesGroupingsNodes == null || !speciesGroupingsNodes.Any()) return new List<Character>();

            IEnumerable<Character> characters = speciesGroupingsNodes.SelectMany(x => CreateCharactersFromSpeciesGrouping(x));
            Characters = characters.ToList();
            return Characters;
        }

        private IEnumerable<Character> CreateCharactersFromSpeciesGrouping(HtmlNode speciesGroupingNode) {
            string speciesGroupingName = speciesGroupingNode.InnerText;
            System.Console.WriteLine($"SpeciesGrouping: {speciesGroupingName}");
            HtmlNodeCollection categoryNodes = speciesGroupingNode.SelectNodes($"//following::h3/span[@class='{HeadlineClass}'][preceding::h2[1]/span[.=\"{speciesGroupingName}\"]]");

            if(categoryNodes == null || !categoryNodes.Any()) return new List<Character>();

            IEnumerable<Character> characters = categoryNodes.SelectMany(x => CreateCharactersFromCategory(speciesGroupingName, x));
            return characters;
        }

        private IEnumerable<Character> CreateCharactersFromCategory(string speciesGroupingName, HtmlNode categoryNode) {
            string categoryName = categoryNode.InnerText;
            System.Console.WriteLine($"Category: {categoryName}");

            string speciesName, characterSelector;
            
            CategoryValue categoryValue = new CategoryValue(CategoryType, categoryName);
            if (speciesGroupingName == "Humans")
            {
                speciesName = "Human";
                characterSelector = $"//following::tr/td[1][preceding::h3[1]/span[.=\"{categoryName}\"]]";
                return CreateCharactersFromSpecies(speciesName, characterSelector, categoryValue, categoryNode);
            }

           
            HtmlNodeCollection speciesNodes = categoryNode.SelectNodes($"//following::h4/span[@class='{HeadlineClass}'][preceding::h3[1]/span[.=\"{categoryName}\"]]");
            if(speciesNodes == null || !speciesNodes.Any()) return new List<Character>();
            return speciesNodes.SelectMany(x => {
                string speciesName = x.InnerText;
                string characterSelector = $"//following::tr/td[1][following::h2/span[.='{StopHeadlineText}']][preceding::h4[1]/span[.=\"{speciesName}\"]][preceding::h3[1]/span[.=\"{categoryName}\"]]";
                return CreateCharactersFromSpecies(speciesName, characterSelector, categoryValue, categoryNode);
            });
        }

        private IEnumerable<Character> CreateCharactersFromSpecies(string speciesName, string characterSelector, CategoryValue categoryValue, HtmlNode categoryNode) 
        {
            System.Console.WriteLine($"Species: {speciesName}");

            HtmlNodeCollection characterNodes = categoryNode.SelectNodes(characterSelector);

            if(characterNodes == null || !characterNodes.Any()) return new List<Character>();
            
            CategoryValue speciesValue = new CategoryValue(SpeciesType, speciesName);
            CategoryValue[] categoryValues = new CategoryValue[] { categoryValue, speciesValue };

            IEnumerable<Character> characters = characterNodes.Select(characterNode => CreateCharacter(categoryValues, characterNode));
            return characters;
        }

        private Character CreateCharacter(CategoryValue[] categoryValues, HtmlNode characterNode) {
            string name = characterNode.InnerText.Trim();
            HtmlNode alternateNameNode = characterNode.SelectSingleNode(".//span[@style='font-size:85%;']");
            string? alternateName = alternateNameNode?.InnerText.Trim();
            string[] names = new string[] {name};
            if(alternateName != null) {
                name = name.Replace(alternateName, "").Trim();
                names.Append(alternateName);
            }
            System.Console.WriteLine($"Name: {name}, Alternate: {alternateName}");

            return new Character(names, categoryValues);
        }
    }
}
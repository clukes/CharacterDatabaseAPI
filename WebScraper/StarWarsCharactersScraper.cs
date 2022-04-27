using HtmlAgilityPack;
using CharacterDatabaseAPI.Models;

namespace CharacterDatabaseAPI.WebScraper
{
    public class StarWarsCharactersScraper : CharactersScraper
    {
        private const string DefaultFileName = "StarWarsCharacters.json";
        private const string WikiPageUrl = "List_of_Star_Wars_characters";
        private const string HeadlineClass = "mw-headline";
        private const string StopHeadlineText = "See also";
        private CategoryType SpeciesCategoryType;
        private CategoryType CharacterCategoryType;
        public StarWarsCharactersScraper(CharacterCollection? characters = null) : base(characters) 
        {
            SpeciesCategoryType = new CategoryType("Species");
            CharacterCategoryType = new CategoryType("Category");
        }
        public override void WriteCharactersToJSONFile(string fileName = DefaultFileName)
        {
            base.WriteCharactersToJSONFile(fileName);
        }

        public override CharacterCollection? ReadCharactersFromJSONFile(string fileName = DefaultFileName)
        {
            return base.ReadCharactersFromJSONFile(fileName);
        }
        public override CharacterCollection? RetrieveCharacters() {
            HtmlDocument document = HTMLRetriever.GetWikiDocument(WikiPageUrl);
            System.Console.WriteLine($"Processing: {WikiPageUrl}");

            HtmlNodeCollection speciesGroupingsNodes = document.DocumentNode.SelectNodes($"//h2/span[@class='{HeadlineClass}'][following::h2/span[.='{StopHeadlineText}']]");

            if(speciesGroupingsNodes == null || !speciesGroupingsNodes.Any()) return null;
            IEnumerable<Character> characters = new List<Character>();
            characters = speciesGroupingsNodes.Take(1).SelectMany(x => CreateCharactersFromSpeciesGrouping(x)).ToList();
        
            IEnumerable<CategoryType> categoryTypes = new List<CategoryType>() { SpeciesCategoryType, CharacterCategoryType };
            Characters = new CharacterCollection("Star Wars", categoryTypes, characters);
            return Characters;
        }

        private IEnumerable<Character> CreateCharactersFromSpeciesGrouping(HtmlNode speciesGroupingNode) {
            string speciesGroupingName = speciesGroupingNode.InnerText;
            System.Console.WriteLine($"SpeciesGrouping: {speciesGroupingName}");
            HtmlNodeCollection categoryNodes = speciesGroupingNode.SelectNodes($"//following::h3/span[@class='{HeadlineClass}'][preceding::h2[1]/span[.=\"{speciesGroupingName}\"]]");

            if(categoryNodes == null || !categoryNodes.Any()) return new List<Character>();

            IEnumerable<Character> characters = categoryNodes.Take(1).SelectMany(x => CreateCharactersFromCategory(speciesGroupingName, x)).ToList();
            return characters;
        }

        private IEnumerable<Character> CreateCharactersFromCategory(string speciesGroupingName, HtmlNode categoryNode) {
            string categoryName = categoryNode.InnerText;
            System.Console.WriteLine($"Category: {categoryName}");

            string speciesName, characterSelector;
            
            CategoryValue characterCategoryValue = new CategoryValue(categoryName);
            CharacterCategoryType.AppendValue(characterCategoryValue);

            CategorySet characterCategorySet = new CategorySet(CharacterCategoryType.Name, characterCategoryValue.Value);
            IEnumerable<CategorySet> categorySets = new List<CategorySet>() { characterCategorySet };
            IEnumerable<Character> characters;
            if (speciesGroupingName == "Humans")
            {
                speciesName = "Human";
                characterSelector = $"//following::tr/td[1][preceding::h3[1]/span[.=\"{categoryName}\"]]";
                characters = CreateCharactersFromSpecies(speciesName, characterSelector, categorySets, categoryNode);
            }
            else
            {
                HtmlNodeCollection speciesNodes = categoryNode.SelectNodes($"//following::h4/span[@class='{HeadlineClass}'][preceding::h3[1]/span[.=\"{categoryName}\"]]");
                if(speciesNodes == null || !speciesNodes.Any()) return new List<Character>();
                characters = speciesNodes.SelectMany(x => {
                    string speciesName = x.InnerText;
                    string characterSelector = $"//following::tr/td[1][following::h2/span[.='{StopHeadlineText}']][preceding::h4[1]/span[.=\"{speciesName}\"]][preceding::h3[1]/span[.=\"{categoryName}\"]]";
                    return CreateCharactersFromSpecies(speciesName, characterSelector, categorySets, categoryNode);
                }).ToList();
            }
            characterCategoryValue.Characters = characters;
            return characters;
        }

        private IEnumerable<Character> CreateCharactersFromSpecies(string speciesName, string characterSelector, IEnumerable<CategorySet> categorySets, HtmlNode categoryNode) 
        {
            System.Console.WriteLine($"Species: {speciesName}");

            HtmlNodeCollection characterNodes = categoryNode.SelectNodes(characterSelector);

            if(characterNodes == null || !characterNodes.Any()) return new List<Character>();
            
            CategoryValue speciesCategoryValue = new CategoryValue(speciesName);
            SpeciesCategoryType.AppendValue(speciesCategoryValue);
            categorySets = categorySets.Append(new CategorySet(SpeciesCategoryType.Name, speciesCategoryValue.Value));

            IEnumerable<Character> characters = characterNodes.Select(characterNode => CreateCharacter(categorySets, characterNode)).ToList();
            speciesCategoryValue.Characters = characters;
            return characters;
        }

        private Character CreateCharacter(IEnumerable<CategorySet> categorySets, HtmlNode characterNode) {
            string name = characterNode.InnerText.Trim();
            HtmlNode alternateNameNode = characterNode.SelectSingleNode(".//span[@style='font-size:85%;']");
            string? alternateName = alternateNameNode?.InnerText.Trim();
            string[] names;
            if(alternateName != null) {
                name = name.Replace(alternateName, "").Trim();
                names = new string[] {name, alternateName};
            }
            else names = new string[] {name};

            System.Console.WriteLine($"Name: {name}, Alternate: {alternateName}");

            Character character = new Character(names, categorySets);
            return character;
        }
    }
}
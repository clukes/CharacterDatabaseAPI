using HtmlAgilityPack;
using CharacterDatabaseAPI.Models;

namespace CharacterDatabaseAPI.WebScraper
{
    public class StarWarsCharactersScraper : CharactersScraper
    {
        private const string Universe = "StarWars";
        private const string WikiPageUrl = "List_of_Star_Wars_characters";
        private const string HeadlineClass = "mw-headline";
        private const string StopHeadlineText = "See also";
        private const string speciesCategoryName = "Species";
        private const string categoryCategoryName = "Category";
        public StarWarsCharactersScraper(ICollection<Character>? characters = null) : base(characters)
        {
            DefaultDirName = "StarWars/";
        }
        public override ICollection<Character>? RetrieveCharacters()
        {
            HtmlDocument document = HTMLRetriever.GetWikiDocument(WikiPageUrl);
            System.Console.WriteLine($"Processing: {WikiPageUrl}");

            HtmlNodeCollection speciesGroupingsNodes = document.DocumentNode.SelectNodes($"//h2/span[@class='{HeadlineClass}'][following::h2/span[.='{StopHeadlineText}']]");

            if (speciesGroupingsNodes == null || !speciesGroupingsNodes.Any()) return null;
            ICollection<Character> characters = new List<Character>();
            characters = speciesGroupingsNodes.Take(1).SelectMany(x => CreateCharactersFromSpeciesGrouping(x)).ToList();

            Characters = characters;
            return characters;
        }

        private ICollection<Character> CreateCharactersFromSpeciesGrouping(HtmlNode speciesGroupingNode)
        {
            string speciesGroupingName = speciesGroupingNode.InnerText;
            System.Console.WriteLine($"SpeciesGrouping: {speciesGroupingName}");
            HtmlNodeCollection categoryNodes = speciesGroupingNode.SelectNodes($"//following::h3/span[@class='{HeadlineClass}'][preceding::h2[1]/span[.=\"{speciesGroupingName}\"]]");

            if (categoryNodes == null || !categoryNodes.Any()) return new List<Character>();

            ICollection<Character> characters = categoryNodes.Take(1).SelectMany(x => CreateCharactersFromCategory(speciesGroupingName, x)).ToList();
            return characters;
        }

        private ICollection<Character> CreateCharactersFromCategory(string speciesGroupingName, HtmlNode categoryNode)
        {
            string categoryName = categoryNode.InnerText;
            System.Console.WriteLine($"Category: {categoryName}");

            string speciesName, characterSelector;

            Dictionary<string, string> categorySets = new Dictionary<string, string>() { { categoryCategoryName, categoryName } };
            ICollection<Character> characters;
            if (speciesGroupingName == "Humans")
            {
                speciesName = "Human";
                characterSelector = $"//following::tr/td[1][preceding::h3[1]/span[.=\"{categoryName}\"]]";
                characters = CreateCharactersFromSpecies(speciesName, characterSelector, categorySets, categoryNode);
            }
            else
            {
                HtmlNodeCollection speciesNodes = categoryNode.SelectNodes($"//following::h4/span[@class='{HeadlineClass}'][preceding::h3[1]/span[.=\"{categoryName}\"]]");
                if (speciesNodes == null || !speciesNodes.Any()) return new List<Character>();
                characters = speciesNodes.SelectMany(x =>
                {
                    string speciesName = x.InnerText;
                    string characterSelector = $"//following::tr/td[1][following::h2/span[.='{StopHeadlineText}']][preceding::h4[1]/span[.=\"{speciesName}\"]][preceding::h3[1]/span[.=\"{categoryName}\"]]";
                    return CreateCharactersFromSpecies(speciesName, characterSelector, categorySets, categoryNode);
                }).ToList();
            }
            return characters;
        }

        private ICollection<Character> CreateCharactersFromSpecies(string speciesName, string characterSelector, Dictionary<string, string> categorySets, HtmlNode categoryNode)
        {
            System.Console.WriteLine($"Species: {speciesName}");

            HtmlNodeCollection characterNodes = categoryNode.SelectNodes(characterSelector);

            if (characterNodes == null || !characterNodes.Any()) return new List<Character>();

            categorySets[speciesCategoryName] = speciesName;

            ICollection<Character> characters = characterNodes.Select(characterNode => CreateCharacter(categorySets, characterNode)).ToList();

            return characters;
        }

        private Character CreateCharacter(Dictionary<string, string> categorySets, HtmlNode characterNode)
        {
            string name = characterNode.InnerText.Trim();
            HtmlNode alternateNameNode = characterNode.SelectSingleNode(".//span[@style='font-size:85%;']");
            string? alternateName = alternateNameNode?.InnerText.Trim();
            HashSet<string> alternateNames = new HashSet<string>();
            if (alternateName != null)
            {
                name = name.Replace(alternateName, "").Trim();
                alternateNames.Add(alternateName);
            }

            System.Console.WriteLine($"Name: {name}, Alternate: {alternateName}");

            Character character = new Character
            {
                Universe = Universe,
                Name = name,
                OtherNames = alternateNames,
                CategorySets = categorySets,
            };
            return character;
        }
    }
}
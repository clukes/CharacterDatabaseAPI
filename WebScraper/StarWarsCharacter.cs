using HtmlAgilityPack;

namespace NameGeneratorAPI.WebScraper
{
    public class StarWarsCharacter
    {
        public StarWarsCharacter(string name, string species, string speciesGrouping, string category, string? alternateName = null) 
        {
            Name = name;
            AlternateName = alternateName;
            Species = species;
            SpeciesGrouping = speciesGrouping;
            Category = category;
        }

        public string Name { get; set; }
        public string? AlternateName { get; set; }
        public string Species { get; set; }
        public string SpeciesGrouping { get; set; }
        public string Category { get; set; }

        public override string ToString() {
            return $"Name: {Name}, AlternateName: {AlternateName}, Species: {Species}, SpeciesGrouping: {SpeciesGrouping}, Category: {Category}";
        }
    }
}
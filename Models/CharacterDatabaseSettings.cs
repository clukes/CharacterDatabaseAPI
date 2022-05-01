namespace CharacterDatabaseAPI.Models 
{
    public class CharacterDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string CharacterCollectionCollectionName { get; set; } = null!;
        public string CategoryTypeCollectionSuffix { get; set; } = null!;
        public string CharacterCollectionSuffix { get; set; } = null!;
        public string[] CollectionPrefixes { get; set; } = null!;
    }
}
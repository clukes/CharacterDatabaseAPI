using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
namespace CharacterDatabaseAPI.Models;
[DynamoDBTable("CharacterDatabase")]
public class Character
{
    [DynamoDBHashKey("Universe")]
    public string Universe { get; set; } = null!;
    [DynamoDBRangeKey("Name")]
    public string Name { get; set; } = null!;
    [DynamoDBProperty("OtherNames")]
    public HashSet<string> OtherNames { get; set; } = null!;
    [DynamoDBProperty("CategorySets")]
    public Dictionary<string,string> CategorySets { get; set; } = null!;
}

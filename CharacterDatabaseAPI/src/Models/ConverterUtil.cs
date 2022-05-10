using Amazon.DynamoDBv2.DocumentModel;

namespace CharacterDatabaseAPI.Models;
public static class ConverterUtil
{
    public static Document StringDictToDoc(IDictionary<string, string> dict)
    {
        Dictionary<string, DynamoDBEntry> newDict = new Dictionary<string, DynamoDBEntry>();
        foreach (var pair in dict)
        {
            newDict.Add(pair.Key, pair.Value);
        }
        Document doc = new Document(newDict);
        return doc;
    }
}

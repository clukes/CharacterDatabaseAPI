using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Microsoft.AspNetCore.Mvc;
using CharacterDatabaseAPI.Models;

namespace CharacterDatabaseAPI.Services;

public class CharacterService : ICharacterService
{
    private readonly AmazonDynamoDBClient _client;
    private readonly IDynamoDBContext _dynamoDBContext;

    public CharacterService()
    {
        _client = new AmazonDynamoDBClient();
        _dynamoDBContext = new DynamoDBContext(_client);
    }

    public async Task<IEnumerable<Character>> GetAsync(string universe)
    {
        AsyncSearch<Character> search = _dynamoDBContext.QueryAsync<Character>(universe);

        return await search.GetRemainingAsync();
    }

    public async Task<Character> GetAsync(string universe, string characterName) =>
        await _dynamoDBContext.LoadAsync<Character>(universe, characterName);

    public async Task SaveAsync(Character character) =>
        await _dynamoDBContext.SaveAsync(character);

    public async Task SaveManyAsync(IEnumerable<Character> characters)
    {
        BatchWrite<Character> batch = _dynamoDBContext.CreateBatchWrite<Character>();
        batch.AddPutItems(characters);
        await batch.ExecuteAsync();
    }

    public async Task DeleteAsync(string universe, string characterName) =>
        await _dynamoDBContext.DeleteAsync<Character>(universe, characterName);

    public async Task<IEnumerable<Character>> SearchAsync(string universe, string? categoryType = null, string? categoryValue = null)
    {
        // Note: You can only query the tables that have a composite primary key (partition key and sort key).

        // 1. Construct QueryFilter
        Expression keyExpression = new Expression()
        {
            ExpressionStatement = "Universe = :universe",
            ExpressionAttributeValues = new Dictionary<string, DynamoDBEntry>() {
                { ":universe", universe },
            }
        };

        Expression filterExpression = new Expression();
        if (categoryType is not null)
        {
            if (categoryValue is null) throw new ArgumentNullException("No category value given.");
            filterExpression.ExpressionStatement = $"#CatSet.#CatType = :categoryValue";
            filterExpression.ExpressionAttributeNames = new Dictionary<string, string>() {
                { "#CatSet", "CategorySets" },
                { "#CatType", categoryType },
            };
            filterExpression.ExpressionAttributeValues = new Dictionary<string, DynamoDBEntry>() {
                { ":categoryValue", categoryValue },
            };
        }

        // 2. Construct QueryOperationConfig
        QueryOperationConfig queryOperationConfig = new QueryOperationConfig
        {
            KeyExpression = keyExpression,
            FilterExpression = filterExpression,
        };

        // 3. Create async search object
        AsyncSearch<Character> search = _dynamoDBContext.FromQueryAsync<Character>(queryOperationConfig);

        // 4. Finally get all the data in a singleshot
        return await search.GetRemainingAsync();
    }
}
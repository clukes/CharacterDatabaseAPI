using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace CharacterDatabaseAPI.Models;

public interface IDocumentModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Id { get; set; }
}
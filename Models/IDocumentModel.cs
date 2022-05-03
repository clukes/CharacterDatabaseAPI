using System.Text.Json.Serialization;

namespace CharacterDatabaseAPI.Models;

public interface IDocumentModel
{
    public string? Id { get; set; }
}
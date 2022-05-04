using CharacterDatabaseAPI.Models;

namespace CharacterDatabaseAPI.Services;

public interface ICharacterService
{
    public Task<IEnumerable<Character>> GetAsync(string universe);
    public Task<Character> GetAsync(string universe, string name);
    public Task SaveAsync(Character character);
    public Task SaveManyAsync(IEnumerable<Character> character);
    public Task DeleteAsync(string universe, string name);
    public Task<IEnumerable<Character>> SearchAsync(string universe, string? categoryType = null, string? categoryValue = null);
}
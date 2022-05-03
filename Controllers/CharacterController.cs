using Microsoft.AspNetCore.Mvc;
using CharacterDatabaseAPI.Models;
using CharacterDatabaseAPI.Services;

namespace CharacterDatabaseAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CharacterController : ControllerBase
{
    private readonly ICharacterService _characterService;

    public CharacterController(ICharacterService characterService)
    {
        _characterService = characterService;
    }

    [Route("get/{universe}")]
    [HttpGet]
    public async Task<IActionResult> Get(string universe)
    {
        var searchResponse = await _characterService.GetAsync(universe);
        return Ok(searchResponse);
    }

    [Route("get/{universe}/{characterName}")]
    [HttpGet]
    public async Task<IActionResult> Get(string universe, string characterName)
    {
        var character = await _characterService.GetAsync(universe, characterName);
        return Ok(character);
    }

    [Route("create")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(Character character)
    {
        await _characterService.SaveAsync(character);
        return Ok();
    }

    [Route("delete/{universe}/{characterName}")]
    [HttpDelete]
    public async Task<IActionResult> Delete(string universe, string characterName)
    {
        await _characterService.DeleteAsync(universe, characterName);
        return Ok();
    }


    [Route("search/{universe}")]
    [HttpGet]
    public async Task<IActionResult> Search(string universe, string? categoryType = null, string? categoryValue = null)
    {
        try
        {
            var searchResponse = await _characterService.SearchAsync(universe, categoryType, categoryValue);
            return Ok(searchResponse);
        }
        catch (ArgumentNullException)
        {
            return BadRequest("categoryType and categoryValue must both be non-null");
        }
    }
}

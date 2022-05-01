using CharacterDatabaseAPI.Models;
using CharacterDatabaseAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CharacterDatabaseAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CharacterController : ControllerBase
{
    private readonly CharacterCollectionService _characterCollectionService;

    public CharacterCollectionController(CharacterCollectionService characterCollectionService) =>
        _characterCollectionService = characterCollectionService;

    [HttpGet]
    public async Task<List<CharacterCollection>> Get() =>
        await _characterCollectionService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<CharacterCollection>> Get(string id)
    {
        var characterCollection = await _characterCollectionService.GetAsync(id);

        if (characterCollection is null)
        {
            return NotFound();
        }

        return characterCollection;
    }

    [HttpPost]
    public async Task<IActionResult> Post(CharacterCollection newCollection)
    {
        await _characterCollectionService.CreateAsync(newCollection);

        return CreatedAtAction(nameof(Get), new { id = newCollection.Id }, newCollection);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, CharacterCollection updatedCollection)
    {
        var characterCollection = await _characterCollectionService.GetAsync(id);

        if (characterCollection is null)
        {
            return NotFound();
        }

        updatedCollection.Id = characterCollection.Id;

        await _characterCollectionService.UpdateAsync(id, updatedCollection);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var characterCollection = await _characterCollectionService.GetAsync(id);

        if (characterCollection is null)
        {
            return NotFound();
        }

        await _characterCollectionService.RemoveAsync(id);

        return NoContent();
    }
}
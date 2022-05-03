using CharacterDatabaseAPI.Models;
using CharacterDatabaseAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CharacterDatabaseAPI.Controllers;

[GenericController]
public class GenericController<TDocumentModel> : ControllerBase, IController<TDocumentModel> where TDocumentModel : IDocumentModel
{
    private readonly ICollectionService<TDocumentModel> _collectionService;

    public GenericController(ICollectionService<TDocumentModel> collectionService, string route)
    {
        _collectionService = collectionService;
    }

    [HttpGet]
    public async Task<List<TDocumentModel>> Get() =>
        await _collectionService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<TDocumentModel>> Get(string id)
    {
        var collection = await _collectionService.GetAsync(id);

        if (collection is null)
        {
            return NotFound();
        }

        return collection;
    }

    [HttpPost]
    public async Task<IActionResult> Post(TDocumentModel newCollection)
    {
        await _collectionService.CreateAsync(newCollection);

        return CreatedAtAction(nameof(Get), new { id = newCollection.Id }, newCollection);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, TDocumentModel updatedCollection)
    {
        var collection = await _collectionService.GetAsync(id);

        if (collection is null)
        {
            return NotFound();
        }

        updatedCollection.Id = collection.Id;

        await _collectionService.UpdateAsync(id, updatedCollection);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var collection = await _collectionService.GetAsync(id);

        if (collection is null)
        {
            return NotFound();
        }

        await _collectionService.RemoveAsync(id);

        return NoContent();
    }
}
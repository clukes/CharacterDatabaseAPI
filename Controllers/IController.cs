using CharacterDatabaseAPI.Models;
using CharacterDatabaseAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CharacterDatabaseAPI.Controllers;

public interface IController<TDocumentModel>
{
    [HttpGet]
    public Task<List<TDocumentModel>> Get();

    [HttpGet("{id:length(24)}")]
    public Task<ActionResult<TDocumentModel>> Get(string id);

    [HttpPost]
    public Task<IActionResult> Post(TDocumentModel newCollection);

    [HttpPut("{id:length(24)}")]
    public Task<IActionResult> Update(string id, TDocumentModel updatedCollection);

    [HttpDelete("{id:length(24)}")]
    public Task<IActionResult> Delete(string id);
}
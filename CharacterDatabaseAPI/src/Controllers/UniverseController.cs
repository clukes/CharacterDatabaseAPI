using Microsoft.AspNetCore.Mvc;
using CharacterDatabaseAPI.Models;
using CharacterDatabaseAPI.Services;

namespace CharacterDatabaseAPI.Controllers;

[Route("api/")]
[ApiController]
public class UniverseController : ControllerBase
{
    private readonly string[] UniverseList = new string[] {
        "StarWars"
    };

    [Route("")]
    [HttpGet]
    public IActionResult GetUniverses() =>
        Ok(UniverseList);

}

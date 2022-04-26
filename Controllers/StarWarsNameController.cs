// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using CharacterDatabaseAPI.Models;

// namespace CharacterDatabaseAPI.Controllers
// {
//     [Route("api/starwars")]
//     [ApiController]
//     public class StarWarsNameController : ControllerBase
//     {
//         private readonly StarWarsNameContext _context;

//         public StarWarsNameController(StarWarsNameContext context)
//         {
//             _context = context;
//         }

//         // GET: api
//         [HttpGet]
//         public async Task<ActionResult<IEnumerable<StarWarsName>>> GetNames()
//         {
//             return await _context.Names.ToListAsync();
//         }

//         // GET: api/RandomName
//         [HttpGet("random")]
//         public async Task<ActionResult<StarWarsName>> GetRandomName()
//         {
//             return await PickRandom(_context.Names);
//         }

//         // GET: api/Humans
//         [HttpGet("category/{category}")]
//         public async Task<ActionResult<IEnumerable<StarWarsName>>> GetNamesInCategory(string category)
//         {
//             return await _context.Names.Where(x => x.Category.Equals(category)).ToListAsync();
//         }

//         // GET: api/Humans/Random
//         [HttpGet("category/{category}/random")]
//         public async Task<ActionResult<StarWarsName>> GetRandomNameInCategory(string category)
//         {
//             var names = _context.Names.Where(x => x.Category.Equals(category));
//             return await PickRandom(names);
//         }

//         // GET: api/species/Trandoshan
//         [HttpGet("species/{species}")]
//         public async Task<ActionResult<IEnumerable<StarWarsName>>> GetNamesInSpecies(string species)
//         {
//             return await _context.Names.Where(x => x.Species.Equals(species)).ToListAsync();
//         }

//         // GET: api/species/Trandoshan/random
//         [HttpGet("species/{species}/random")]
//         public async Task<ActionResult<StarWarsName>> GetRandomNameInSpecies(string species)
//         {
//             var names = _context.Names.Where(x => x.Species.Equals(species));
//             return await PickRandom(names);
//         }

//         // POST: api/populate
//         [HttpPost("populate")]
//         public async Task<IActionResult> PopulateNames()
//         {
//             try {
//                 await _context.PopulateNames();
//                 return Ok();
//             } catch(Exception e) {
//                 return BadRequest(e.Message);
//             }
//         }

//         private static async Task<StarWarsName> PickRandom(IQueryable<StarWarsName> names) {
//             int randomIndex = new Random().Next(names.Count());
//             var nameList = await names.Skip(randomIndex).Take(1).ToListAsync();
//             return nameList.First();
//         }

//     }
// }
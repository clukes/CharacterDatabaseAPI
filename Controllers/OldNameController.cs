// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using CharacterDatabaseAPI.Models;

// namespace CharacterDatabaseAPI.Controllers
// {
//     public abstract class NameController : ControllerBase, INameController
//     {
//         private readonly INameContext _context;

//         public NameController(INameContext context)
//         {
//             _context = context;
//         }

//         public async Task<ActionResult<IEnumerable<IName>>> GetNames()
//         {
//             return await _context.Names.ToListAsync();
//         }

//         public async Task<IName> GetRandomName()
//         {
//             return await PickRandom(_context.Names);
//         }

//         public async Task<ActionResult<IEnumerable<ICategoryType>>> GetCategoryTypes() 
//         {
//             return await _context.CategoryTypes.ToListAsync();
//         }

//         public async Task<ActionResult<IEnumerable<ICategoryValue>>> GetCategoryValues(long categoryTypeID) 
//         {
//             return await _context.CategoryValues.Where(x => x.CategoryType.Id == categoryTypeID).ToListAsync();
//         }

//         public Task<ActionResult<IEnumerable<IName>>> GetNamesInCategory(long categoryTypeID, long categoryValueID)
//         {
//             return await _context.Names.Where(x => x.Category.Equals(category)).ToListAsync();
//         }

//         public IName GetRandomNameInCategory(string category)
//         {
//             var names = _context.Names.Where(x => x.Category.Equals(category));
//             int randomIndex = new Random().Next(names.Count());
//             IName name = names.ElementAt(randomIndex);

//             return name;
//         }

//         private static async Task<IName> PickRandom(IQueryable<IName> names) {
//             int randomIndex = new Random().Next(names.Count());
//             var nameList = await names.Skip(randomIndex).Take(1).ToListAsync();
//             return nameList.First();
//         }
//     }
// }
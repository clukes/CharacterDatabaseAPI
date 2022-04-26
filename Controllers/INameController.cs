// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using CharacterDatabaseAPI.Models;

// namespace CharacterDatabaseAPI.Controllers
// {
//     public interface INameController
//     {
//         public Task<ActionResult<IEnumerable<IName>>> GetNames();

//         public Task<ActionResult<IName>> GetRandomName();

//         public Task<ActionResult<IEnumerable<ICategoryType>>> GetCategoryTypes();

//         public Task<ActionResult<IEnumerable<ICategoryValue>>> GetCategoryValues(string categoryType);

//         public Task<ActionResult<IEnumerable<IName>>> GetNamesInCategory(string categoryType, string categoryValue);

//         public Task<ActionResult<IName>> GetRandomNameInCategory(string categoryType, string categoryValue);

//     }
// }
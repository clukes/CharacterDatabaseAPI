using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NameGeneratorAPI.Models;

namespace NameGeneratorAPI.Controllers
{
    public interface INameController
    {
        public Task<ActionResult<IEnumerable<IName>>> GetNames();

        public IName GetRandomName();

        public Task<ActionResult<IEnumerable<IName>>> GetNamesInCategory(string category);

        public IName GetRandomNameInCategory(string category);

    }
}
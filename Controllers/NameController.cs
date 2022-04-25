using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NameGeneratorAPI.Models;

namespace NameGeneratorAPI.Controllers
{
    public abstract class NameController : ControllerBase, INameController
    {
        private readonly INameContext _context;

        public NameController(INameContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<IName>>> GetNames()
        {
            return await _context.Names.ToListAsync();
        }

        public IName GetRandomName()
        {
            int randomIndex = new Random().Next(_context.Names.Count());
            IName name = _context.Names.ElementAt(randomIndex);

            return name;
        }

        public async Task<ActionResult<IEnumerable<IName>>> GetNamesInCategory(string category)
        {
            return await _context.Names.Where(x => x.Category.Equals(category)).ToListAsync();
        }

        public IName GetRandomNameInCategory(string category)
        {
            var names = _context.Names.Where(x => x.Category.Equals(category));
            int randomIndex = new Random().Next(names.Count());
            IName name = names.ElementAt(randomIndex);

            return name;
        }
    }
}
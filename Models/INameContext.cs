using Microsoft.EntityFrameworkCore;

namespace NameGeneratorAPI.Models
{
    public interface INameContext
    {
        public DbSet<IName> Names { get; set; }
    }
}
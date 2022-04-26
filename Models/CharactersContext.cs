using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace CharacterDatabaseAPI.Models
{
    public class NameCategoryValueContext : DbContext
    {
        public NameCategoryValueContext(DbContextOptions<NameCategoryValueContext> options)
            : base(options)
        {
        }

        public DbSet<NameCategoryValue> NameCategoryValues { get; set; } = null!;
    }
}
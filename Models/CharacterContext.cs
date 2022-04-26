using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace CharacterDatabaseAPI.Models
{
    public class CharacterContext : DbContext
    {
        public CharacterContext(DbContextOptions<CharacterContext> options)
            : base(options)
        {
        }

        public DbSet<Character> Characters { get; set; } = null!;
    }
}
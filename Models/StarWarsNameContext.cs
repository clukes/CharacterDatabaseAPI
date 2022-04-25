using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace NameGeneratorAPI.Models
{
    public class StarWarsNameContext : DbContext
    {
        private bool populated = false;
        public DbSet<StarWarsName> Names { get; set; } = null!;
        
        public StarWarsNameContext(DbContextOptions<StarWarsNameContext> options)
            : base(options)
        {

        }
        
        public async Task PopulateNames() {
            if(populated) throw new ArgumentException("Already populated");
            await RepopulateNames();
        }

        public async Task RepopulateNames() {
            string jsonText = await File.ReadAllTextAsync("Data/StarWarsRealCharacters.json");
            StarWarsName[]? names = JsonSerializer.Deserialize<StarWarsName[]>(jsonText);
            if (names == null) throw new ArgumentNullException("No names");
            Names.AddRange(names);
            await SaveChangesAsync();
            populated = true;
        }
    }
}
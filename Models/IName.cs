namespace NameGeneratorAPI.Models
{
    public interface IName
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
    }
}
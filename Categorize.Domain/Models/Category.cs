using Categorize.Domain.Interfaces;

namespace Categorize.Domain.Models
{
    public class Category
    {
        public string Name { get; set; }
        public string Rule { get; set; }
        public Func<ITrade, DateTime, bool> CompiledRule { get; set; }
    }
}

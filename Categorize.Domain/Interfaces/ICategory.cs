namespace Categorize.Domain.Interfaces
{
    public interface ICategory
    {
        string Name { get; }
        bool IsMatch(ITrade trade, DateTime referenceDate);
    }
}

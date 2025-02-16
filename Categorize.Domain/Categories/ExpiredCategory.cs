using Categorize.Domain.Interfaces;

namespace Categorize.Domain.Categories
{
    public class ExpiredCategory : ICategory
    {
        public string Name => "EXPIRED";

        public bool IsMatch(ITrade trade, DateTime referenceDate)
        {
            return (referenceDate - trade.NextPaymentDate).Days > 30;
        }
    }
}

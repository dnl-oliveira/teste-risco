using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Categorize.Domain.Interfaces
{
    public interface ICategoryRule
    {
        string Category { get; }
        bool IsMatch(ITrade trade, DateTime referenceDate);
    }
}

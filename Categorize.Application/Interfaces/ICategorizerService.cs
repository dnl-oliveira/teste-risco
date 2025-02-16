using Categorize.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Categorize.Application.Interfaces
{
    public interface ICategorizerService
    {
        Task<string> Categorize(ITrade trade, DateTime referenceDate);
    }
}

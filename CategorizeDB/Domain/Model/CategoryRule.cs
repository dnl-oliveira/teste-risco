using CategorizeDB.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CategorizeDB.Domain.Model
{
    public class CategoryRule
    {
        public string Name { get; set; }
        public string Rule { get; set; }
        public Func<ITrade, DateTime, bool> CompiledRule { get; set; }
    }
}

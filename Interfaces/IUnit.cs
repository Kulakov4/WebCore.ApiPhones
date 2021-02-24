using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCore.ApiPhones.Models;

namespace WebCore.ApiPhones.Interfaces
{
    public interface IUnit
    {
        public Task<IEnumerable<UnitClass>> GetUnits();
    }
}

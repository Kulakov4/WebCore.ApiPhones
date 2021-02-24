using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCore.ApiPhones.Models;

namespace WebCore.ApiPhones.Interfaces
{
    public interface IPhoneBook
    {
        public Task<IEnumerable<PhoneBookRaw>> GetPhoneBookByLastName(string lastName);
        public Task<IEnumerable<PhoneBookRaw>> GetPhoneBookByPhone(string phone);
        public Task<IEnumerable<PhoneBookRaw>> GetPhoneBookByUnitID(string UnitID);
    }
}

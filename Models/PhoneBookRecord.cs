using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCore.ApiPhones.Models
{
    public class PhoneBookRecord
    {
        public string PhysicalId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        public string UnitId { get; set; }
        public string Unit { get; set; }
        public string Post { get; set; }
        public string Office { get; set; }
        public List<Phone> Phones { get; set; }
        public List<Email> Emails { get; set; }
    }
}

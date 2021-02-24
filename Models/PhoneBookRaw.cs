using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCore.ApiPhones.Models
{
    public class PhoneBookRaw
    {
        public string PhysicalId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        public string UnitId { get; set; }
        public string Unit { get; set; }
        public string Post { get; set; }
        public string Office { get; set; }
        public int PhoneId { get; set; }
        public string PhoneNumber { get; set; }
        public string RawPhone { get; set; }
        public int? EmailId { get; set; }
        public string? email { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCore.ApiPhones.Models
{
    public class Phone
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string RawPhone { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCore.ApiPhones.Interfaces
{
    public interface IPhoto
    {
        public Task<byte[]> GetPhoto(string id);
    }
}

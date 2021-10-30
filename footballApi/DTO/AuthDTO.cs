using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace footballApi.DTO
{
    public class AuthDTO
    {
        public int? Id { get; set; }
        public String NameId { get; set; }
        public bool IsAdmin { get; set; }

        public String Type { get; set; }
    }
}

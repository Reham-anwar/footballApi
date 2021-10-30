using footballApi.DTO.AbstractClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace footballApi.DTO
{
    public class AdminRegisterDTO : AppUserDTO
    {
        public String Password { get; set; }
        public String ConfPassword { get; set; }
    }
}

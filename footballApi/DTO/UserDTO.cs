using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace footballApi.DTO
{
    public class UserDTO
    {
        public String UserName { get; set; }
        public String Token { get; set; }

        public String UserType { get; set; } = "User";
    }
}

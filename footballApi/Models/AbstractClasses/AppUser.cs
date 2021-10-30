using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace footballApi.Models.AbstractClasses
{
   abstract public class AppUser
    {
        public int Id { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String UserName { get; set; }
        public int Age { get; set; }
        public String Email { get; set; }

        [JsonIgnore]
        public byte[] PasswordHash { set; get; }

        [JsonIgnore]
        public byte[] PasswordSalt { set; get; }

        public byte[] GetPasswordSalt()
        {
            return PasswordSalt;
        }

        public byte[] GetPasswordHash()
        {
            return PasswordHash;
        }

        

    }
}

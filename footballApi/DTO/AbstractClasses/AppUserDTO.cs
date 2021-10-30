using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace footballApi.DTO.AbstractClasses
{
    public abstract class AppUserDTO
    {

        [Required]
        public String UserName { get; set; }
        public int Id { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public int Age { get; set; }
        public String Email { get; set; }
       
    }
}

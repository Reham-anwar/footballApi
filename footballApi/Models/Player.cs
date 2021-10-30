using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace footballApi.Models
{
    public class Player
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is reuired")]
        [StringLength(100)]
        public String Name { get; set; }

        [Required(ErrorMessage = "Birthdate is reuired")]
        public DateTime DOB { get; set; }

        //
        [Required(ErrorMessage = "Nationality is reuired")]
        public String Nationality { get; set; }

        //img
        public string img { get; set; }

        
        public virtual Team Team { get; set; }

        public int? TeamId { get; set; }

    }
}

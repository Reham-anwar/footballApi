using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace footballApi.Models
{
    public class Team
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is reuired")]
        [StringLength(100)]
        public String Name { get; set; }

        [Required(ErrorMessage = "Coach_Name is reuired")]
        [StringLength(100)]
        public String Coach_Name { get; set; }

        [Required(ErrorMessage = "Country is reuired")]
        [StringLength(100)]
        public string Country { get; set; }

        [Required(ErrorMessage ="Date is required")]
        public DateTime Foundation_Date { get; set; }

        //img
        public string img { get; set; }

        public virtual ICollection<Player> Players { get; set; }
           = new HashSet<Player>();

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    [Table("Role")]
    public class Rola
    {
        [Key]
        public int id_roli { get; set; }
        [Required]
        [Column(TypeName = "varchar(8)")]
        public string nazwa { get; set; }

        public virtual ICollection<RolaUzytkownika> uzytkownicy { get; set; }
    }
}

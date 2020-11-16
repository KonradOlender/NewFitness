using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Areas.Identity.Data;

namespace WebApplication.Models
{
    [Table("Posilki")]
    public class Posilek
    {
        [Key]
        public int id_posilku { get; set; }
        [Required]
        [Column(TypeName = "varchar(40)")]
        public string nazwa { get; set; }
        [Required]
        public int kalorie { get; set; }
        [Column(TypeName = "varchar(1000)")]
        public string opis { get; set; }
        [Required, ForeignKey("uzytkownik")]
        public int id_uzytkownika { get; set; }

        public virtual ICollection<PosilekSzczegoly> skladniki { get; set; }
        public virtual Uzytkownik uzytkownik { get; set; }
    }
}

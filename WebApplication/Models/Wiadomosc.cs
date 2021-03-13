using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Areas.Identity.Data;

namespace WebApplication.Models
{
    [Table ("Wiadomosci")]
    public class Wiadomosc
    {
        [Key]
        public int id { get; set; }
        [Required, ForeignKey("nadawca")]
        public int id_nadawcy { get; set; }
        [Required, ForeignKey("odbiorca")]
        public int id_odbiorcy { get; set; }
        [Required]
        public DateTime data { get; set; }
        [Required]
        public string tekst { get; set; }

        public virtual Uzytkownik nadawca { get; set; }
        public virtual Trening odbiorca { get; set; }

    }
}

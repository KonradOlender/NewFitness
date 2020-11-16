using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Areas.Identity.Data;

namespace WebApplication.Models
{
    [Table("HistoriaUzytkownikow")]
    public class HistoriaUzytkownika
    {
        [ForeignKey("uzytkownik")]
        public int id_uzytkownika { get; set; }
        [Required]
        public DateTime data { get; set; }
        [Required]
        public double waga { get; set; }
        [Required]
        public int wzrost { get; set; }

        public virtual Uzytkownik uzytkownik { get; set; }
    }
}

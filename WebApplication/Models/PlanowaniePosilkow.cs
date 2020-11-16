using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Areas.Identity.Data;

namespace WebApplication.Models
{
    [Table("PlanowanePosilki")]
    public class PlanowaniePosilkow
    {
        [Required]
        public int id { get; set; }
        [Required, ForeignKey("uzytkownik")]
        public int id_uzytkownika { get; set; }
        [Required, ForeignKey("posilek")]
        public int id_posilku { get; set; }
        [Required]
        public DateTime data { get; set; }

        public virtual Uzytkownik uzytkownik { get; set; }
        public virtual Posilek posilek { get; set; }

    }
}

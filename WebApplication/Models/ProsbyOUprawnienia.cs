using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Areas.Identity.Data;

namespace WebApplication.Models
{
    [Table("ProsbyOUprawnienia")]
    public class ProsbyOUprawnienia
    {
        [Required, ForeignKey("uzytkownik")]
        public int id_uzytkownika { get; set; }
        [Required, ForeignKey("rola")]
        public int id_roli { get; set; }
        public String prosba_pisemna { get; set; }

        public virtual Rola rola { get; set; }
        public virtual Uzytkownik uzytkownik { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Areas.Identity.Data;

namespace WebApplication.Models
{
    [Table("RoleUzytkownikow")]
    public class RolaUzytkownika
    {
        [Required, ForeignKey("uzytkownik")]
        public int id_uzytkownika { get; set; }
        [Required, ForeignKey("rola")]
        public int id_roli { get; set; }

        public virtual Rola rola { get; set; }
        public virtual Uzytkownik uzytkownik { get; set; }
    }
}

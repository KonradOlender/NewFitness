using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Areas.Identity.Data;

namespace WebApplication.Models
{
    [Table("PlanowaneTreningi")]
    public class PlanowanieTreningow
    {
        [Key]
        public int id { get; set; }
        [Required, ForeignKey("uzytkownik")]
        public int id_uzytkownika { get; set; }
        [Required, ForeignKey("trening")]
        public int id_treningu { get; set; }
        [Required]
        public DateTime data { get; set; }

        public virtual Uzytkownik uzytkownik { get; set; }
        public virtual Trening trening { get; set; }

    }
}

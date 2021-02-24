using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Areas.Identity.Data;

namespace WebApplication.Models
{
    [Table("OcenyTreningow")]
    public class OcenaTreningu
    {
        [Required, ForeignKey("oceniajacy")]
        public int id_uzytkownika { get; set; }
        [Required, ForeignKey("trening")]
        public int id_treningu { get; set; }
        [Required]
        public double ocena { get; set; }
        public virtual Uzytkownik oceniajacy { get; set; }
        public virtual Trening trening { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    [Table("Skladniki")]
    public class Skladnik
    {
        [Key]
        public int id_skladnika { get; set; }
        [Required]
        public int waga { get; set; }
        [Required]
        [Column(TypeName = "varchar(20)")]
        public int nazwa { get; set; }
        [Required]
        public int kalorie { get; set; }
        [Required, ForeignKey("kategoria")]
        public int id_kategorii { get; set; }

        public virtual KategoriaSkladnikow kategoria { get; set; }
        public virtual ICollection<PosilekSzczegoly> posilki { get; set; }
    }
}

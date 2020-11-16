using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    [Table("KategorieTreningow")]
    public class KategoriaTreningu
    {
        [Key]
        public int id_kategorii { get; set; }
        [Required]
        [Column(TypeName = "varchar(15)")]
        public string nazwa { get; set; }

        public virtual ICollection<Trening> treningi { get; set; }
    }
}

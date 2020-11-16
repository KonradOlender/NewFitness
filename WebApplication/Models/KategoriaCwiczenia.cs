using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    [Table("KategorieCwiczen")]
    public class KategoriaCwiczenia
    {
        [Key]
        public int id_kategorii { get; set; }
        [Required]
        [Column(TypeName = "varchar(15)")]
        public string nazwa { get; set; }


        public virtual ICollection<Cwiczenie> cwiczenia { get; set; }


        public override string ToString()
        {
            return nazwa;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    [Table("Cwiczenia")]
    public class Cwiczenie
    {
        [Key]
        public int id_cwiczenia { get; set; }
        [Required]
        [Column(TypeName = "varchar(20)")]
        public string nazwa { get; set; }
        [Column(TypeName = "varchar(1000)")]
        [Required]
        public string opis { get; set; }
        public int spalone_kalorie { get; set; }
        [Required]
        [ForeignKey("kategoria")]
        public int id_kategorii { get; set; }


        public virtual KategoriaCwiczenia kategoria { get; set; }
        public virtual ICollection<TreningSzczegoly> treningi { get; set; }

        public override string ToString()
        {
            return nazwa;
        }

    }
}

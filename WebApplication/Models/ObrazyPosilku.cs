using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class ObrazyPosilku
    {
        [Key]
        public int id_obrazu { get; set; }
        [Required]
        public byte[] obraz { get; set; }
        [Required]
        [ForeignKey("posilek")]
        public int id_posilku { get; set; }

        public virtual Posilek posilek { get; set; }
        public string GetImageDataUrl()
        {
            string imageBase64Data = Convert.ToBase64String(obraz);
            return string.Format("data:image/jpg;base64,{0}", imageBase64Data);
        }

    }
}

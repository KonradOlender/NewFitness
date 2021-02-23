using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{

    public class ObrazyTreningu
    {
        [Key]
        public int id_obrazu { get; set; }
        [Required]
        public byte[] obraz { get; set; }
        [Required][ForeignKey("trening")]
        public int id_treningu { get; set; }

        public virtual Trening trening { get; set; }

        public string GetImageDataUrl()
        {
            string imageBase64Data = Convert.ToBase64String(obraz);
            return string.Format("data:image-training" + id_obrazu.ToString() + "/jpg;base64,{0}", imageBase64Data);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Areas.Identity.Data;

namespace WebApplication.Models
{
    public class ObrazProfilowe
    {
        [Key]
        [ForeignKey("uzytkownik")]
        public int id_uzytkownika { get; set; }
        [Required]
        public byte[] obraz { get; set; }

        public virtual Uzytkownik uzytkownik { get; set; }

        public string GetImageDataUrl()
        {
            string imageBase64Data = Convert.ToBase64String(obraz);
            return string.Format("data:image-profilepic" + id_uzytkownika.ToString() + "/jpg;base64,{0}", imageBase64Data);
        }
    }
}

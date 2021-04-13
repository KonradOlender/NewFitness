using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Models
{
    public class Notyfikacje
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Uzytkownik")]
        public int UserId { get; set; }
        public bool Viewed { get; set; }
    }
}

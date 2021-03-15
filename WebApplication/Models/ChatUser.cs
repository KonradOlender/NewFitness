using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Models
{
    public class ChatUser
    {
        [Required, ForeignKey("Uzytkownik")]
        public int UserId { get; set; }
        public Uzytkownik User { get; set; }
        [Required, ForeignKey("Chat")]
        public int ChatId { get; set; }
        public Chat Chat { get; set; }

    }
}

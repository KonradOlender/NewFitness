using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Models
{
    [Table("messages")]
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
        public int ChatId { get; set; }
        public Chat Chat { get; set; }

    }
}

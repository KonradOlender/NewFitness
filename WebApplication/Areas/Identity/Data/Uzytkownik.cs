using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebApplication.Models;

namespace WebApplication.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the Uzytkownik class
    [Table("AspNetUsers")]
    public class Uzytkownik : IdentityUser<int>
    {
        public string imie { get; set; }
        public string login { get; set; }
        public int limit { get; set; }

        public virtual ICollection<RolaUzytkownika> role { get; set; }
        public virtual ICollection<Trening> treningi { get; set; }
        public virtual ICollection<PlanowanieTreningow> planowaneTreningi { get; set; }
        public virtual ICollection<Posilek> posilki { get; set; }
        public virtual ICollection<PlanowaniePosilkow> planowanePosilki { get; set; }
        public virtual ICollection<HistoriaUzytkownika> historiaUzytkownika { get; set; }
        public virtual ICollection<Ocena> oceny { get; set; }
        public virtual ICollection<ProsbyOUprawnienia> prosby { get; set; }

        public override String ToString()
        {
            return login;
        }
    
    }
}

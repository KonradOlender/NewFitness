using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication.Areas.Identity.Data;
using WebApplication.Models;

namespace WebApplication.Data
{
    public class IdentityContext<TUser>
    : IdentityUserContext<TUser, int>
        where TUser : Uzytkownik
    {
        public IdentityContext(DbContextOptions<IdentityContext<TUser>> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApplication.Areas;
using WebApplication.Areas.Identity.Data;
using WebApplication.Data;
using WebApplication.Entities;
using WebApplication.Hubs;
using WebApplication.Services;

namespace WebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<MyContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("IdentityContextConnection")));

            services.AddDefaultIdentity<Uzytkownik>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<MyContext>().AddDefaultTokenProviders();

            //chat
            services.AddSignalR();

            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.AddHostedService<Notifications>();
            services.AddRazorPages();
            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddSingleton<IHostedService, Notifications>();
            /*services.Configure<MyContext>(o =>
            {
                o.Database.Migrate();
            });*/

            /*services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });*/

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //chat
            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chatHub");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "deleteRole",
                    pattern: "rolaUzytkownika/deletepermissions/{role_id}/{user_id}",
                    defaults: new { controller = "RolaUzytkownika", action = "DeleteConfirmed" });

                endpoints.MapControllerRoute(
                    name: "exerciseDelete",
                    pattern: "trening/deleteexercise/{idt}/{idc}",
                    defaults: new { controller = "Trening", action = "DeleteExercise" });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");


                endpoints.MapRazorPages();
            });
            
        }
    }
}

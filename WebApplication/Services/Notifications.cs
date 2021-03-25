using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApplication.Areas;
using WebApplication.Data;
using WebApplication.Entities;
using WebApplication.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace WebApplication.Services
{
    public class Notifications : BackgroundService
    {
        IEmailSender emailSender;
        private readonly IServiceScopeFactory scopeFactory;
        private bool active = false;
        Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;

        public Notifications(
            IOptions<EmailSettings> emailSettings, IServiceScopeFactory scopeFactory,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            emailSender = new EmailSender(emailSettings, env);
            this.scopeFactory = scopeFactory;
            this._env = env;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<MyContext>();
                /*string filePath = Path.Combine(_env.WebRootPath, "messages/Notification.html");
                string messageHtml = System.IO.File.ReadAllText(filePath);*/

                while (!stoppingToken.IsCancellationRequested)
                {
                    if (!active) {
                        await Task.Delay(60000, stoppingToken); 
                    }
                    DateTime currentTime = DateTime.Now;
                    DateTime dateAhead = currentTime.Add(TimeSpan.FromMinutes(30));
                    DateTime dateBehind = currentTime.Subtract(TimeSpan.FromMinutes(30));
                    if (!dbContext.planowaneTreningi.Any(t => t.data < dateAhead && dateBehind < t.data && t.notification_sent == false))
                        await Task.Delay(60000, stoppingToken);

                    List<PlanowanieTreningow> listy = await dbContext.planowaneTreningi
                                                               .Include(t => t.uzytkownik)
                                                               .Where(t => t.data < dateAhead && dateBehind < t.data && t.notification_sent == false)
                                                               .ToListAsync();

                    foreach (PlanowanieTreningow trening in listy)
                    {
                        trening.notification_sent = true;
                        //string messageToSent = string.Format(messageHtml, trening.data);
                        string message = string.Format("<h1>Masz zaplanowany trening o godzinie {0}</h1><br>" +
                            "<p> Po więcej informacji zaloguj się na nasz protal</p>  ", trening.data);
                        await emailSender.SendEmailAsync(
                            trening.uzytkownik.Email,
                            "Zaplanowany trening",
                            message);
                        await dbContext.SaveChangesAsync();
                    }
                    await Task.Delay(60000, stoppingToken);
                }
            }   
            
        }
    }
}

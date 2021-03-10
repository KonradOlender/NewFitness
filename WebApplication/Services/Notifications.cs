using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApplication.Areas;
using WebApplication.Data;
using WebApplication.Entities;
using WebApplication.Models;
using Microsoft.Extensions.DependencyInjection;

namespace WebApplication.Services
{
    public class Notifications : BackgroundService
    {
        MyContext dbContext;
        IEmailSender emailSender;
        
        /*public Notifications(
            IOptions<EmailSettings> emailSettings,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            emailSender = new EmailSender(emailSettings,env)
        }*/

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            dbContext = new MyContext();
            var serviceCollection = new Microsoft.Extensions.DependencyInjection.ServiceCollection();
            var _di = serviceCollection.BuildServiceProvider();
            emailSender = _di.GetRequiredService<IEmailSender>();
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                DateTime currentTime = DateTime.Now;
                List<PlanowanieTreningow> listy = dbContext.planowaneTreningi
                                                           .Where(t => TimeSpan.FromTicks(t.data.Ticks - currentTime.Ticks).TotalMinutes < 60)
                                                           .Include(t => t.uzytkownik)
                                                           .ToList();

                foreach(PlanowanieTreningow trening in listy)
                {
                    string message = string.Format("<h1>Masz zaplanowany trening o godzinie {0}</h1>", trening.data);
                    await emailSender.SendEmailAsync(
                        trening.uzytkownik.Email,
                        "Zaplanowany trening",
                        message);
                }
                await Task.Delay(5, stoppingToken);
            }

        }
    }
}

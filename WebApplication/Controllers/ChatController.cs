using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication.Data;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class ChatController : Controller
    {
        private readonly MyContext _context;
        public ChatController(MyContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                int userid = int.Parse(User.Identity.GetUserId());
                var user = _context.uzytkownicy.Single(e => e.Id == userid);
                var chat = _context.chatUsers.Where(e => e.UserId == userid).Include(e => e.Chat).ToList();
                ViewBag.chat = chat;
                ViewBag.user = user;

                this.isAdmin();
            }
            
            return View();
        }


        [HttpGet]
        public IActionResult Room(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                int userid = int.Parse(User.Identity.GetUserId());
                var user = _context.uzytkownicy.Single(e => e.Id == userid);
                var chat = _context.chats.Include(e => e.Messages)
                        .FirstOrDefault(e => e.Id == id);
                ViewBag.chat = chat;
                ViewBag.user = user;
            }
            return View();
        }
        //create message
        [HttpPost]
        public IActionResult Room(string name, string text, int chatId)
        {
            Message message = new Message();
            message.Name = name;
            message.Text = text;
            message.Timestamp = DateTime.Now;
            message.ChatId = chatId;
            _context.messages.Add(message);
            _context.SaveChanges();

            return RedirectToAction("Room", new { id = chatId });
        }

        [HttpPost]
        public IActionResult CrateRoom(int userId, int specialistId)
        {
            var chatUser = _context.chatUsers.Where(e => e.UserId == userId).ToList();
            var chatSpecialist = _context.chatUsers.Where(e => e.UserId == specialistId).ToList();
            List<Chat> listaChatUserOne = new List<Chat>();
            List<Chat> listaChatUserTwo = new List<Chat>();
            foreach (var item in chatUser)
            {
                var usersChats = _context.chats.Single(e => e.Id == item.ChatId);
                listaChatUserOne.Add(usersChats);
            }

            
            foreach (var item in chatSpecialist)
            {
                var usersChats = _context.chats.Single(e => e.Id == item.ChatId);
                listaChatUserTwo.Add(usersChats);
            }
            foreach (var item in listaChatUserOne)
            {
                foreach (var itemm in listaChatUserTwo)
                {
                    if (item.Id == itemm.Id)
                    {
                        return Redirect("/Chat/Room/" + item.Id);
                    }
                }

            }
            RoomController rc = new RoomController(_context);
            rc.CreateRoom(userId, specialistId);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult DeleteRoom(int chatId)
        {
            RoomController rc = new RoomController(_context);
            rc.DeleteRoom(chatId);
            return Redirect("/UserHub/Admin");
        }


        private bool isAdmin()
        {
            if (User.Identity.IsAuthenticated)
            {
                int userId = int.Parse(User.Identity.GetUserId());
                List<RolaUzytkownika> usersRoles = _context.RolaUzytkownika.Where(k => k.id_uzytkownika == userId).Include(c => c.rola).ToList();
                foreach (var usersRole in usersRoles)
                    if (usersRole.rola.nazwa == "admin")
                    {
                        ViewBag.ifAdmin = true;
                        return true;
                    }
            }
            return false;
        }

    }
}

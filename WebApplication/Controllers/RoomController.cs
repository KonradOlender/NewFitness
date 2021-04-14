using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Data;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class RoomController
    {
        private readonly MyContext _context;
        public RoomController(MyContext context)
        {
            _context = context;
        }

        public void CreateRoom(int userId, int specialistId)
        {
            var user = _context.uzytkownicy.Single(e=>e.Id == userId);
            var specialist = _context.uzytkownicy.Single(e => e.Id == specialistId);

            
            Chat chat = new Chat();
            chat.NameOne = user.imie;
            chat.NameTwo = specialist.imie;
            chat.FirstId = userId;
            chat.SecendId = specialistId;
            chat.Users.Add(new ChatUser
            {
                UserId = userId
            });
            chat.Users.Add(new ChatUser
            {
                UserId = specialistId
            });
            _context.Add(chat);
            _context.SaveChanges();
        }
        public void DeleteRoom(int chatId)
        {
            Chat chat = _context.chats.Single(e=>e.Id == chatId);
            _context.Remove(chat);
            _context.SaveChanges();
        }
    }
}

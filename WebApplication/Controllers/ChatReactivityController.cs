using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebApplication.Data;
using WebApplication.Hubs;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ChatReactivityController : Controller
    {
        private IHubContext<ChatHub> _chat;

        public ChatReactivityController(IHubContext<ChatHub> chat)
        {
            _chat = chat;
        }

        [HttpPost("[action]/{connectionId}/{roomName}")]
        public async Task<IActionResult> JoinRoom(string connectionId, string roomName)
        {
            await _chat.Groups.AddToGroupAsync(connectionId, roomName);
            return Ok();
        }

        [HttpPost("[action]/{connectionId}/{roomName}")]
        public async Task<IActionResult> LeaveRoom(string connectionId, string roomName)
        {
            await _chat.Groups.RemoveFromGroupAsync(connectionId, roomName);
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SendMessage(int chatId, string message, string roomName, [FromServices] MyContext _context)
        {
            Message Message = new Message();
            Message.Name = User.Identity.Name;
            Message.Text = message;
            Message.Timestamp = DateTime.Now;
            Message.ChatId = chatId;

            _context.messages.Add(Message);
            _context.SaveChanges();

            await _chat.Clients.Group(roomName).SendAsync("RecieveMessage", Message);
            return Ok();
        }

    }
}

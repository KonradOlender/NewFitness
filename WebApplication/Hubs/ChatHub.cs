using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Hubs
{
    public class ChatHub : Hub
    {
        //string userId;

        ////na razie message to string bo nie ma tego w bazie
        ////zamiast .All to .Client ale trzeba jakoś pobrać to konkretne id do kogo ta wiadomość
        //public async Task SendMessage(string message) =>
        //    await Clients.Client(userId).SendAsync("ReceiveMessage", message);
    }
}

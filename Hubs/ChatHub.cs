using ChatApp.Models;
using ChatApp.Models.Context;
using ChatApp.Models.Entities;
using ChatApp.Services;
using ChatApp.ViewModels;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Hubs
{
    public class ChatHub : Hub
    {
        private readonly DataContext _dataContext;
        private readonly ChatContext _chatContext;

        public ChatHub(ChatContext chatContext, DataContext dataContext)
        {
            _dataContext = dataContext;
            _chatContext = chatContext;
        }
        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync(
                 "ReceivePost", "ChatKewl",
                     DateTimeOffset.UtcNow,
                     "Hello do you like music?");
            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async Task PostMessage(string name, string text)
        {
            var message = new ChatsViewModel
            {
                Message = text,
                Time = DateTime.Now
            };

            await Clients.All.SendAsync(
                "ReceivePost",
               
                message.Message,
                message.Time);
        }
    }
}


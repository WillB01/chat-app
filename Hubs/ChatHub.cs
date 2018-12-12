using ChatApp.Models;
using ChatApp.Models.Context;
using ChatApp.Models.Entities;
using ChatApp.Services;
using ChatApp.Services.ViewModelService;
using ChatApp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatService _viewModelService;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public ChatHub(IChatService viewModelService, IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _viewModelService = viewModelService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;

        }
        public override async Task OnConnectedAsync()
        {
            
            var user = _httpContextAccessor.HttpContext.User;
            var loggedinUser = _userService.GetloggedinUser(user);
            var test = await _userService.GetloggedinUser(user);
            var t = _viewModelService.GetUserChats(test).Select(p => p.Message);
               




            await Clients.Caller.SendAsync(
                 "ReceiveMessage", "ChatKewl",
                     DateTimeOffset.UtcNow,
                   t
                     );
            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string name, string text)
        {
            var message = new ChatsViewModel
            {
                Message = text,
                Time = DateTime.Now
            };

            await Clients.All.SendAsync(
                "ReceiveMessage",
               
                "kewl",
                message.Time);
        }
    }
}


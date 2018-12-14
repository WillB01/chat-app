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
    public class ChatHub : Hub<IChatClient>
    {
        private readonly IChatService _viewModelService;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static List<UserConnections> _uList = new List<UserConnections>();


        public ChatHub(IChatService viewModelService, IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _viewModelService = viewModelService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;

        }
        public override async Task OnConnectedAsync()
        {
           
            //var user = _httpContextAccessor.HttpContext.User;
            //var loggedinUser = _userService.GetloggedinUser(user);
            //var test = await _userService.GetloggedinUser(user);
            //var t = _viewModelService.GetUserChats(test).Select(p => p.Message);
            //var hej = _httpContextAccessor.HttpContext.User.Identities;

            var us = new UserConnections();
            us.UserName = Context.User.Identity.Name;
            us.ConnectionID = Context.ConnectionId;
            _uList.Add(us);





            await Clients.Caller.ReceiveMessage(
                    "ChatKewl",
                     DateTime.Now
                   
                     );
            
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string name, string text)
        {
            var message = new ChatsViewModel
            {
                Message = text,
                Time = DateTime.Now
            };

            await Clients.All.ReceiveMessage(
                "kewl",
                message.Time);
        }

        public async Task SendPrivateMessage(string user, string message)
        {
            var userTosend =_uList.Where(u => u.UserName == user).Select(p => p.ConnectionID).First();
            var test = userTosend;
           await Clients.Client(userTosend).ReceiveMessage(message);
            //await Clients.All.ReceiveMessage(message);
           
        }

        public string GetConnectionId() => Context.ConnectionId;

        //public Task SendMessageToGroup(string message)
        //{
        //    return Clients.Group("SignalR Users").SendAsync("ReceiveMessage", message);
        //}
    }
}


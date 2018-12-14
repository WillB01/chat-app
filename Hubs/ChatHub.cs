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
        private static HashSet<UserConnections> _uList = new HashSet<UserConnections>();


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
            var test = Clients.All;
            var k = test;
            _uList.Add(us);





            await Clients.Caller.ReceiveMessage(
                    "ChatKewl",
                     DateTime.Now
                   
                     );
            
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            //await Groups.RemoveFromGroupAsync(Context.ConnectionId, Clients.Caller.ToString());
            await base.OnDisconnectedAsync(exception);
        }

        //public async Task AddToGroup(string groupName)
        //{
        //    var userTosend = _uList.Where(u => u.UserName == groupName).Select(p => p.ConnectionID).FirstOrDefault();
        //    await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        //    await Groups.AddToGroupAsync(userTosend, groupName);

        //    await Clients.Group(groupName).ReceiveMessage("Send", $"{Context.ConnectionId} has joined the group {groupName}.");
        //}

        //public async Task RemoveFromGroup(string groupName)
        //{
        //    await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

        //    await Clients.Group(groupName).ReceiveMessage("Send", $"{Context.ConnectionId} has left the group {groupName}.");
        //}

        

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

            var userTosend =_uList.Where(u => u.UserName == user).Select(p => p.ConnectionID).FirstOrDefault();
            var test = userTosend;

            await Clients.Client(userTosend).ReceiveMessage(message, DateTime.Now);
            //await Clients.Group(user).ReceiveMessage(message);
            //await Clients.All.ReceiveMessage(message);
           
        }

        public string GetConnectionId() => Context.ConnectionId;
        public string GetHistory(string user)
        {
            return "kewdddl";
        }

        //public Task SendMessageToGroup(string message)
        //{
        //    return Clients.Group("SignalR Users").SendAsync("ReceiveMessage", message);
        //}
    }
}


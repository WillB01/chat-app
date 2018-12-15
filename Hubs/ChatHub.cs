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
        private readonly IChatService _chatService;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static HashSet<UserConnections> _uList = new HashSet<UserConnections>();


        public ChatHub(IChatService chatService, IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _chatService = chatService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;

        }
        public override async Task OnConnectedAsync()
        {
            var us = new UserConnections
            {
                UserName = Context.User.Identity.Name,
                ConnectionID = Context.ConnectionId
            };
         
            _uList.Add(us);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendPrivateMessage(string user, string message)
        {
            var id = await _userService.GetUserId(user);
            var loggedinUserName = await Task.Run(() => Context.User.Identity.Name);
            var idLogged = await _userService.GetUserId(loggedinUserName);
            //var userLogged = await Task.Run(() =>_httpContextAccessor.HttpContext.User.Identities.Select(p => p.ID));
            await _chatService.SaveConversation(idLogged, id, message, DateTime.Now);

            var userTosend = await Task.Run(() => _uList
            .Where(u => u.UserName == user)
            .Select(p => p.ConnectionID)
            .FirstOrDefault());

            await Clients.Client(userTosend).ReceiveMessage(message, DateTime.Now);
            await Clients.Caller.ReceiveMessage(message, DateTime.Now);
        }

        public string GetConnectionId() => Context.ConnectionId;

        public async Task<IEnumerable<ChatsViewModel>[]> GetHistory(string value)
        {
            var user = _httpContextAccessor.HttpContext.User;
            var loggedinUser = await _userService.GetloggedinUser(user);
            var conversation = await _chatService.GetUserConversation(loggedinUser, value);

         
            return conversation;
        }

       
    }
}


using ChatApp.Services;
using ChatApp.Services.FriendService;
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
        private readonly IFriendService _friendService;
        private  HashSet<UserConnections> _uList = new HashSet<UserConnections>();

        public IUserIdProvider _UserIdProvider { get; }

        public ChatHub(IChatService chatService, IUserService userService, 
            IHttpContextAccessor httpContextAccessor, IFriendService friendService, IUserIdProvider _userIdProvider)
        {
            _chatService = chatService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _friendService = friendService;
            _UserIdProvider = _userIdProvider;
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
            var toSendId = await _userService.GetUserId(user);
            var loggedinUserName = await Task.Run(() => Context.User.Identity.Name);
            var idLogged = await _userService.GetUserId(loggedinUserName);

            await _chatService.SaveConversation(idLogged, toSendId, message, DateTime.Now);

            await Clients.User(toSendId).ReceiveMessage(message, DateTime.Now, false, loggedinUserName);
            await Clients.Caller.ReceiveMessage(message, DateTime.Now, true, user);
        }

        public string GetUserName() => Context.User.Identity.Name;

        public async Task<ChatsViewModel[]> GetHistory(string value)
        {
            var user = _httpContextAccessor.HttpContext.User;
            var loggedinUser = await _userService.GetloggedinUser(user);
            var conversation = await _chatService.GetUserConversation(loggedinUser, value);

            return conversation;
        }
    }
}
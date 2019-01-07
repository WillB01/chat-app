using ChatApp.Services;
using ChatApp.Services.FriendService;
using ChatApp.ViewModels;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace ChatApp.Hubs
{
    public class ChatHub : Hub<IChatClient>
    {
        private readonly IChatService _chatService;
        private readonly IUserService _userService;
        private readonly IFriendService _friendService;

        public ChatHub(IChatService chatService, IUserService userService, IFriendService friendService)
        {
            _chatService = chatService;
            _userService = userService;
            _friendService = friendService;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendPrivateMessage(string user, string message)
        {
            var toSendId = await _userService.GetUserId(user);
            var loggedInUser = await _userService.GetloggedinUser();

            await _chatService.SaveConversation(loggedInUser.Id, toSendId, message, DateTime.Now);

            await Clients.User(toSendId).ReceiveMessage(message, DateTime.Now, false, loggedInUser.UserName);
            await Clients.Caller.ReceiveMessage(message, DateTime.Now, true, user);
        }

        public string GetUserName() => Context.User.Identity.Name;

        public async Task<MessageVM[]> GetHistory(string value)
        {
            var loggedinUser = await _userService.GetloggedinUser();
            var conversation = await _chatService.GetUserConversation(loggedinUser, value);

            return conversation;
        }


        public async Task AddFriendsToGroup(string groupName,string[] friendsToAdd)
        {

        }
    }
}
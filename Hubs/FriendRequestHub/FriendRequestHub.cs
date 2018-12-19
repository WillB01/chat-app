using ChatApp.Services;
using ChatApp.Services.FriendService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Hubs.FriendRequestHub
{
    public class FriendRequestHub : Hub<IFriendClient>
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFriendService _friendService;


        public FriendRequestHub(IChatService chatService, IUserService userService,
            IHttpContextAccessor httpContextAccessor, IFriendService friendService)
        {
           
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _friendService = friendService;
          
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.ReceiveFriendRequest(false);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendFriendRequest(string user, string message)
        {
            await Clients.Caller.ReceiveFriendRequest(true);
        }

        
    }
}

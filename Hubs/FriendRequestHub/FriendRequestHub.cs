using ChatApp.Services;
using ChatApp.Services.FriendRequestService;
using ChatApp.Services.FriendService;
using ChatApp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace ChatApp.Hubs.FriendRequestHub
{
    public class FriendRequestHub : Hub<IFriendClient>
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFriendService _friendService;
        private readonly IFriendRequestService _friendRequestService;

        public FriendRequestHub(IChatService chatService, IUserService userService,
            IHttpContextAccessor httpContextAccessor, IFriendService friendService, IFriendRequestService friendRequestService)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _friendService = friendService;
            _friendRequestService = friendRequestService;
        }

        public override async Task OnConnectedAsync()
        {
            var user = await _userService.GetloggedinUser();

            var requests = await _friendRequestService.CheckFriendRequest(user);

            var hasRequests = requests.Length == 0 ? false : true;
            await Clients.Caller.ReceiveFriendRequest(hasRequests, requests);

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

        public async Task SendUserResponse(FriendRequestVM response)
        {
            //var user = await _userService.GetloggedinUser();
            //response.ToUser = user.Id;
            //response.ToUserName = response.ToUserName;

            if (response.HasAccepted == true)
            {
                await _friendRequestService.AcceptFriendRequest(response);
            }
            else if (response.HasAccepted == false)
            {
                await _friendRequestService.DeclineFriendRequest(response);
            }
            await Clients.User(response.FromUser).RecieveUserResponse(response.FromUserName, response.FromUser);
        }

        public async Task<FriendRequestVM[]> CheckFriendRequests()
        {
            var user = await _userService.GetloggedinUser();
            var requests = await _friendRequestService.CheckFriendRequest(user);
            return requests;
        }
    }
}
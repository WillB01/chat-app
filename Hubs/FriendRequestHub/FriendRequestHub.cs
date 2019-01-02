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
            var friends = await _friendService.GetFriends(user);

            var hasRequests = requests.Length == 0 ? false : true;
            await Clients.Caller.ReceiveFriendRequest(hasRequests, requests, friends);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendUserResponse(FriendRequestVM response)
        {
            await SaveToDb(response);
            await SendToLoggedinUser(response);
            await SendToOtherUser(response);
        }

        public async Task SaveToDb(FriendRequestVM response)
        {
            if (response.HasAccepted == true)
            {
                await _friendRequestService.AcceptFriendRequest(response);
                await _friendService.AddNewFriend(response);
            }
            else if (response.HasAccepted == false)
            {
                await _friendRequestService.DeclineFriendRequest(response);
            }
        }

        public async Task<FriendRequestVM[]> CheckFriendRequests(IdentityUserVM user)
        {
            var toUser = await _userService.GetUserByUserName(user.UserName);
            var requests = await _friendRequestService.CheckFriendRequest(toUser);
            return requests;
        }

        public async Task SendToLoggedinUser(FriendRequestVM response)
        {
            var user = await _userService.GetloggedinUser();
            var userHistory = await CheckFriendRequests(user);
            var userHasRequests = userHistory.Length == 0 ? false : true;
            var userFriends = await _friendService.GetFriends(user);
            await Clients.Caller.ReceiveFriendRequest(userHasRequests, userHistory, userFriends, response.HasAccepted);
        }

        public async Task SendToOtherUser(FriendRequestVM response)
        {
            var toUser = await _userService.GetUserByUserName(response.FromUserName);
            var toUserhistory = await CheckFriendRequests(toUser);
            var toUserHasRequests = toUserhistory.Length == 0 ? false : true;
            var toUserFriends = await _friendService.GetFriends(toUser);
            await Clients.User(response.FromUser).ReceiveFriendRequest(toUserHasRequests, toUserhistory, toUserFriends, response.HasAccepted);
        }
    }
}
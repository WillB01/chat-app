using ChatApp.Hubs.FriendRequestHub;
using ChatApp.Models.Entities;
using ChatApp.Services.FriendService;
using ChatApp.ViewModels;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Services.FriendRequestService
{
    public class FriendRequestService : IFriendRequestService
    {
        private readonly ChatContext _chatContext;
        private readonly IUserService _userService;
        private readonly IFriendService _friendService;
        private IHubContext<FriendRequestHub, IFriendClient> _friendRequestHubContext;

        public FriendRequestService(ChatContext chatContext, IUserService userService, IHubContext<FriendRequestHub, IFriendClient> friendRequestHubContext, IFriendService friendService)
        {
            _chatContext = chatContext;
            _userService = userService;
            _friendRequestHubContext = friendRequestHubContext;
            _friendService = friendService;
        }

        public async Task<FriendRequestVM[]> CheckFriendRequest(IdentityUserVM user) // todo add Name to DB
        {
            var requests = await  _chatContext.FriendRequest
           .Where(p => p.ToUser == user.Id && p.HasAccepted == null)
           .Select(e => new FriendRequestVM
           {
               FromUser = e.FromUser,
               FromUserName = e.FromUserName,
               ToUser = e.ToUser,
               ToUserName = user.UserName
           }).ToArrayAsync();

            return requests;
        }

        public async Task AcceptFriendRequest(FriendRequestVM friendRequest)
        {
            var update = await _chatContext.FriendRequest
                .Where(p => p.FromUser == friendRequest.FromUser && p.ToUser == friendRequest.ToUser)
                .FirstOrDefaultAsync();

            update.HasAccepted = true;

            await _chatContext.SaveChangesAsync();
        }

        public async Task DeclineFriendRequest(FriendRequestVM friendRequest)
        {
            var update = await _chatContext.FriendRequest
                .Where(p => p.FromUser == friendRequest.FromUser && p.ToUser == friendRequest.ToUser)
                .FirstOrDefaultAsync();

            update.HasAccepted = false;

            await _chatContext.SaveChangesAsync();
        }

        public async Task SendFriendRequest(FriendRequestVM friendRequest)
        {

            var loggedInUserHasAlreadySent = await _chatContext.FriendRequest
                .Where(p => p.FromUser == friendRequest.FromUser)
                .AnyAsync(e => e.ToUser == friendRequest.ToUser);
            var loggedInUserHasAlreadyReceived = await _chatContext.FriendRequest
                .Where(p => p.FromUser == friendRequest.ToUser)
                .AnyAsync(e => e.ToUser == friendRequest.FromUser);

            var sendFriendRequest = true ? !loggedInUserHasAlreadySent && !loggedInUserHasAlreadyReceived : false;

            if (sendFriendRequest)
            {
                var dbModel = new FriendRequest
                {
                    FromUser = friendRequest.FromUser,
                    ToUser = friendRequest.ToUser,
                    FromUserName = friendRequest.FromUserName,
                    ToUserName = friendRequest.ToUserName
                    
                };
               

                _chatContext.FriendRequest.Add(dbModel);

                await _chatContext.SaveChangesAsync();

                var signalRModel = new FriendRequestVM[] { friendRequest };
                await _friendRequestHubContext.Clients.User(friendRequest.ToUser)
                    .ReceiveFriendRequest(true, signalRModel);
              

            }




        }








    }
}
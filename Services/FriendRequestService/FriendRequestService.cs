using ChatApp.Hubs.FriendRequestHub;
using ChatApp.Models.Entities;
using ChatApp.Models.Identity;
using ChatApp.Services.FriendService;
using ChatApp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Services.FriendRequestService
{
    public class FriendRequestService : IFriendRequestService
    {
        private readonly ChatContext _chatContext;
        private readonly IUserService _userService;
        private readonly IFriendService _friendService;
        IHubContext<FriendRequestHub, IFriendClient> _friendRequestHubContext;
       

        public FriendRequestService(ChatContext chatContext, IUserService userService, IHubContext<FriendRequestHub, IFriendClient> friendRequestHubContext, IFriendService friendService)
        {
            _chatContext = chatContext;
            _userService = userService;
            _friendRequestHubContext = friendRequestHubContext;
            _friendService = friendService;
        }

        public async Task<FriendRequestVM[]> CheckFriendRequest(IdentityUserVM user) // todo add Name to DB
        {
          
            var requests = await Task.Run( () =>_chatContext.FriendRequest
            .Where(p => p.ToUser == user.Id && p.HasAccepted == null )
            .Select(e => new FriendRequestVM
            {
                FromUser = e.FromUser,
                FromUserName = e.FromUserName,
                ToUser = e.ToUser,
                ToUserName = user.UserName

            }).ToArray());
           
            return requests;
            
        }


        public async Task AcceptFriendRequest(FriendRequestVM friendRequest)
        {
            var update = _chatContext.FriendRequest
                .Where(p => p.FromUser == friendRequest.FromUser && p.ToUser == friendRequest.ToUser)
                .FirstOrDefault();

            update.HasAccepted = true;
                
            await _chatContext.SaveChangesAsync();

            await _friendService.AddNewFriend(friendRequest);
        }

        public async Task DeclineFriendRequest(FriendRequestVM friendRequest)
        {
            var theRequest = _chatContext.FriendRequest
               .Where(p => p.FromUser == friendRequest.FromUser && p.ToUser == friendRequest.ToUser)
               .FirstOrDefault();
            var dbModel = new FriendRequest
            {
                FromUser = theRequest.FromUser,
                ToUser = theRequest.ToUser,
                HasAccepted = false
            };

            _chatContext.FriendRequest.Update(dbModel);
            await _chatContext.SaveChangesAsync();
        }

        public async Task SendFriendRequest(FriendRequestVM friendRequest)
        {
            var fromUser = await _userService.GetloggedinUser();

            var signalRModel = new FriendRequestVM[] { friendRequest };

            var dbModel = new FriendRequest
            {
                FromUser = friendRequest.FromUser,
                ToUser = friendRequest.ToUser,
                FromUserName = friendRequest.FromUserName
                
            };


            var getModel = _chatContext.FriendRequest
                .Where(p => p.FromUser == dbModel.FromUser);
            var checkIfSend = getModel.Any(p => p.ToUser == dbModel.ToUser);

            if (!checkIfSend)
            {
                
                await _friendRequestHubContext.Clients.User(friendRequest.ToUser)
               .ReceiveFriendRequest(true, signalRModel);

                _chatContext.FriendRequest.Add(dbModel);
                    
              
                await _chatContext.SaveChangesAsync();
            }
            
            

        }

        

    }
}

using ChatApp.Models.Entities;
using ChatApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Services.FriendRequestService
{
    public class FriendRequestService : IFriendRequestService
    {
        private readonly ChatContext _chatContext;
        


        public FriendRequestService(ChatContext chatContext)
        {
            _chatContext = chatContext;
        }

        public async Task<FriendRequestVM[]> CheckFriendRequest(IdentityUserVM user) // todo add Name to DB
        {
            var requests = await Task.Run(() => _chatContext.FriendRequest
            .Where(p => p.ToUser == user.Id)
            .Select(e => new FriendRequestVM
            {
                FromUser = e.FromUser

            }).ToArray());

            return requests;
            
        }


        public async Task AcceptFriendRequest(FriendRequestVM friendRequest)
        {
            var theRequest = _chatContext.FriendRequest
                .Where(p => p.FromUser == friendRequest.FromUser && p.ToUser == friendRequest.ToUser)
                .FirstOrDefault();
            var dbModel = new FriendRequest
            {
                FromUser = theRequest.FromUser,
                ToUser = theRequest.ToUser,
                HasAccepted = true
            };

            _chatContext.FriendRequest.Update(dbModel);
            await _chatContext.SaveChangesAsync();
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
            var dbModel = new FriendRequest
            {
                FromUser = friendRequest.FromUser,
                ToUser = friendRequest.ToUser,
            };
           await Task.Run( () =>_chatContext.FriendRequest.Add(dbModel));
           await _chatContext.SaveChangesAsync();

        }

    }
}

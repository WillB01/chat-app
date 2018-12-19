using ChatApp.Models.Entities;
using ChatApp.Models.Identity;
using ChatApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Services.FriendRequestService
{
    public interface IFriendRequestService
    {
        Task SendFriendRequest(FriendRequestVM friendRequest);
        Task AcceptFriendRequest(FriendRequestVM friendRequest);
        Task DeclineFriendRequest(FriendRequestVM friendRequest);
        Task<FriendRequestVM[]> CheckFriendRequest(IdentityUserVM user);

    }
}

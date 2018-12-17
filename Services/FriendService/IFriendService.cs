using ChatApp.Models.Identity;
using ChatApp.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatApp.Services.FriendService
{
    public interface IFriendService
    {
        Task<FriendsViewModel[]> GetFriends(IdentityUserVM user);
    }
}
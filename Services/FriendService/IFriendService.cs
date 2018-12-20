using ChatApp.ViewModels;
using System.Threading.Tasks;

namespace ChatApp.Services.FriendService
{
    public interface IFriendService
    {
        Task<FriendsVM[]> GetFriends(IdentityUserVM user);

        Task AddNewFriend(FriendRequestVM friendRequest);
    }
}
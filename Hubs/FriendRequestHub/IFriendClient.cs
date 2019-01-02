using ChatApp.ViewModels;
using System.Threading.Tasks;

namespace ChatApp.Hubs.FriendRequestHub
{
    public interface IFriendClient
    {
        Task ReceiveFriendRequest(bool sentRequest);

        Task ReceiveFriendRequest(bool sentRequest, string fromUser);

        Task ReceiveFriendRequest(bool sentRequest, FriendRequestVM[] requests);

        Task ReceiveFriendRequest(bool sentRequest, FriendRequestVM[] requests, FriendsVM[] friends);

        Task ReceiveFriendRequest(bool sentRequest, FriendRequestVM[] requests, FriendsVM[] friends, bool? hasAccepted);

        Task ReceiveFriendRequest(string fromUserName, string fromUserId);

        Task RecieveUserResponse(string fromUserName, string fromUserId);
    }
}
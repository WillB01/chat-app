using ChatApp.ViewModels;
using System.Threading.Tasks;

namespace ChatApp.Hubs.FriendRequestHub
{
    public interface IFriendClient
    {
        Task ReceiveFriendRequest(bool sentRequest);

        Task ReceiveFriendRequest(bool sentRequest, string fromUser);

        Task ReceiveFriendRequest(bool sentRequest, FriendRequestVM[] requests);

        Task RecieveUserResponse(string fromUserName, string fromUserId);
    }
}
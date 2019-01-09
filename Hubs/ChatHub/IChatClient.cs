using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatApp.Hubs
{
    public interface IChatClient
    {
        Task ReceiveMessage(string user, DateTimeOffset time, IEnumerable<string> message);

        Task ReceiveMessage(string message, DateTime time, bool isLoggedIn);

        Task ReceiveMessage(string message, DateTime time, bool isLoggedIn, string fromFriend);

        Task ReceiveMessage(string message);

        Task ReceiveMessage(string ConnectionId, string groupName);

        Task ReceiveMessage(string sendFromId, string userId, string sendFromName, string userName, string message);

        Task ReceiveGroupInvite(string groupName);

        Task ReceiveGroupInvite(string groupName, bool willJoin);

        Task GroupReceiveMessage(string message);


        Task GroupReceiveMessage(string message, string fromUser, DateTime time, string group);
    }
}
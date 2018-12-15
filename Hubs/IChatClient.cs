using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Hubs
{
    public interface IChatClient
    {
        Task ReceiveMessage(string user, DateTimeOffset time, IEnumerable<string> message);
        Task ReceiveMessage(string message, DateTime time);
        Task ReceiveMessage(string message);
        Task ReceiveMessage(string ConnectionId, string groupName);
        Task ReceiveMessage(string sendFromId, string userId, string sendFromName, string userName, string message);

        
    }
}

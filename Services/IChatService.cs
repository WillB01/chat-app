using ChatApp.Models;
using ChatApp.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Services
{
    public interface IChatService
    {
        IAsyncEnumerable<Chat> Chats { get; }
        IEnumerable<Chat> GetUserChats(AppUser user);
    }
}

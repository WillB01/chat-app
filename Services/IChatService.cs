using ChatApp.Models;
using ChatApp.Models.Entities;
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
        Task<IQueryable<IEnumerable<string>>> GetUserChats(AppUser user, AppUser receiver);
        Task<IQueryable<IEnumerable<string>>> GetUserConversation(AppUser loggedinUser, string getSecondUser);
    }
}

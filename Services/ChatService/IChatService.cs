using ChatApp.Models.Identity;
using ChatApp.ViewModels;
using System;
using System.Threading.Tasks;

namespace ChatApp.Services
{
    public interface IChatService
    {
        Task<ChatsViewModel[]> GetUserConversation(IdentityUserVM loggedinUser, string getSecondUser);

        Task SaveConversation(string userLoggedin, string userId, string message, DateTime time);
    }
}
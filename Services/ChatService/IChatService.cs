using ChatApp.ViewModels;
using System;
using System.Threading.Tasks;

namespace ChatApp.Services
{
    public interface IChatService
    {
        Task<MessageVM[]> GetUserConversation(IdentityUserVM loggedinUser, string getSecondUser);
        Task SaveConversation(string userLoggedin, string userId, string message, DateTime time);
    }
}
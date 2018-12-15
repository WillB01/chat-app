using ChatApp.Models;
using ChatApp.Models.Entities;
using ChatApp.Models.Identity;
using ChatApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Services
{
    public interface IChatService
    {
        Task<IEnumerable<ChatsViewModel>[]> GetUserConversation(AppUser loggedinUser, string getSecondUser);
        Task SaveConversation(string userLoggedin ,string userId, string message, DateTime time);
    }
}

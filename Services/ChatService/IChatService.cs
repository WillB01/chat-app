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
        Task<IQueryable<IEnumerable<string>>> GetUserConversation(AppUser loggedinUser, string getSecondUser);
        Task SaveConversation(string userLoggedin ,string userId, string message, DateTime time);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Models;
using ChatApp.Models.Entities;
using ChatApp.Models.Identity;
using ChatApp.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Services
{
    public class ChatService : IChatService
    {
        private readonly ChatContext _chatContext;
        private readonly IUserService _userService;

        public ChatService(ChatContext chatContext, IUserService userService)
        {
            _chatContext = chatContext;
            _userService = userService;
        }

        public async Task<IQueryable<IEnumerable<string>>> GetUserConversation(AppUser loggedinUser, string getSecondUser)
        {
            var sendToId = await _userService.GetUserId(getSecondUser);

            var result = await Task.Run(() => _chatContext.PrivateMessage
               .Include(p => p.Chat)
               .Where(p => p.UserId == loggedinUser.Id)
               .Select(e => e.Chat.PrivateMessage.Where(i => i.ToUserId.ToString() == sendToId))
               .Select(f => f.Select(k => k.Chat))
               .Select(e => e.Select(b => b.Message)));

            var result2 = await Task.Run(() => _chatContext.PrivateMessage
               .Include(p => p.Chat)
               .Where(p => p.UserId == sendToId)
               .Select(e => e.Chat.PrivateMessage.Where(i => i.ToUserId.ToString() == loggedinUser.Id))
               .Select(f => f.Select(k => k.Chat))
               .Select(e => e.Select(b => b.Message)));

            var hej = result.Concat(result2);



            return hej;
        }

        public async Task SaveConversation(string userLoggedin, string userId, string message, DateTime time)
        {
            var viewModel = new PrivateMessage
            {
                UserId = userLoggedin,
                ToUserId = userId,
               
            };
            viewModel.Chat = new Chat
            {
                Message = message,
                Time = time,
                IdentityId = userLoggedin,
            };
           
            _chatContext.AddRange(viewModel);
            await _chatContext.SaveChangesAsync();

        }
    }
}

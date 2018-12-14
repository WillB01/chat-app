using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Models;
using ChatApp.Models.Entities;
using ChatApp.Models.Identity;
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

        public IAsyncEnumerable<Chat> Chats => _chatContext.Chat.ToAsyncEnumerable();

        public async Task<IQueryable<IEnumerable<string>>> GetUserChats(AppUser user, AppUser receiver)
        {
            
            var result = await Task.Run(() => _chatContext.PrivateMessage
                .Include(p => p.Chat)
                .Where(p => p.UserId == user.Id)
                .Select(e => e.Chat.PrivateMessage.Where(i => i.ToUserId.ToString() == "db6b816c-ebd1-4f18-a765-88545257f1c8"))
                .Select(f => f.Select(k => k.Chat))
                .Select(e => e.Select(b => b.Message)));
            return result;
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
            return result;
        }
    }
}

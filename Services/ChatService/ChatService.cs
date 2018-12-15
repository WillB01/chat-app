using ChatApp.Models.Entities;
using ChatApp.Models.Identity;
using ChatApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<ChatsViewModel[]> GetUserConversation(AppUser loggedinUser, string getSecondUser)
        {
            var sendToId = await _userService.GetUserId(getSecondUser);

            var fromUser = await Task.Run(() => _chatContext.PrivateMessage
              .Include(p => p.Chat)
              .Where(p => p.UserId == loggedinUser.Id && p.ToUserId == sendToId)
              .Select((b) =>
              new ChatsViewModel
              {
                  Message = b.Chat.Message,
                  Time = b.Chat.Time,
                  IsLoggedin = true,
                  IdentityId = b.Chat.IdentityId
              }).ToArray());

            var toUser = await Task.Run(() => _chatContext.PrivateMessage
              .Include(p => p.Chat)
              .Where(p => p.UserId == sendToId && p.ToUserId == loggedinUser.Id)
              .Select((b) =>
              new ChatsViewModel
              {
                  Message = b.Chat.Message,
                  Time = b.Chat.Time,
                  IsLoggedin = false,
                  IdentityId = b.Chat.IdentityId
              }).ToArray());

            var conversation = fromUser.Concat(toUser).ToArray().OrderBy(e => e.Time).ToArray();

            return conversation;
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
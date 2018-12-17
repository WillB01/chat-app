using ChatApp.Models.Entities;
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

        public async Task<MessageVM[]> GetUserConversation(IdentityUserVM loggedinUser, string getSecondUser)
        {
            var toUserId = await _userService.GetUserId(getSecondUser);

            var fromUser = await Task.Run(() => _chatContext.Conversation
              .Include(p => p.Message)
              .Where(p => p.UserId == loggedinUser.Id && p.ToUserId == toUserId)
              .Select((b) =>
              new MessageVM
              {
                  Message = b.Message.Message1,
                  Time = b.Message.Time,
                  IsLoggedin = true,
                  IdentityId = b.Message.IdentityId
              }).ToArray());

            var toUser = await Task.Run(() => _chatContext.Conversation
              .Include(p => p.Message)
              .Where(p => p.UserId == toUserId && p.ToUserId == loggedinUser.Id)
              .Select((b) =>
              new MessageVM
              {
                 
                  Message = b.Message.Message1,
                  Time = b.Message.Time,
                  IsLoggedin = false,
                  IdentityId = b.Message.IdentityId
              }).ToArray());

            var conversation = fromUser.Concat(toUser).ToArray().OrderBy(e => e.Time).ToArray();

            return conversation;
        }

        public async Task SaveConversation(string userLoggedin, string userId, string message, DateTime time)
        {
            var viewModel = new Conversation
            {
                UserId = userLoggedin,
                ToUserId = userId,
            };
            viewModel.Message = new Message
            {
                Message1 = message,
                Time = time,
                IdentityId = userLoggedin,
            };

            _chatContext.AddRange(viewModel);
            await _chatContext.SaveChangesAsync();
        }
    }
}
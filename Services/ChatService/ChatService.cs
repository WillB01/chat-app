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

        public async Task<IEnumerable<ChatsViewModel>[]> GetUserConversation(AppUser loggedinUser, string getSecondUser)
        {
            var sendToId = await _userService.GetUserId(getSecondUser);

            var loggedInUserMsg = await Task.Run(() => _chatContext.PrivateMessage
               .Include(p => p.Chat)
               .Where(p => p.UserId == loggedinUser.Id)
               .Select(e => e.Chat.PrivateMessage.Where(i => i.ToUserId.ToString() == sendToId))
               .Select(f => f.Select((b) =>
               new ChatsViewModel
               {
                   Message = b.Chat.Message,
                   Time = b.Chat.Time,
                   Identity = b.Chat.Identity,
                   IdentityId = b.Chat.IdentityId
               })).ToArray());

            var sendToUserMsg = await Task.Run(() => _chatContext.PrivateMessage
              .Include(p => p.Chat)
              .Where(p => p.UserId == sendToId)
              .Select(e => e.Chat.PrivateMessage.Where(i => i.UserId.ToString() == sendToId))
              .Select(f => f.Select((b) => 
              new ChatsViewModel
              {
                  Message = b.Chat.Message,
                  Time = b.Chat.Time,
                  Identity = b.Chat.Identity,
                  IdentityId = b.Chat.IdentityId
              })).ToArray());


            var conversation = loggedInUserMsg.Concat(sendToUserMsg)
                .OrderBy(p => p.FirstOrDefault().Time)
                .ToArray();

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

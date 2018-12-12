using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Models;
using ChatApp.Models.Identity;

namespace ChatApp.Services
{
    public class ChatService : IChatService
    {
        private readonly ChatContext _chatContext;

        public ChatService(ChatContext chatContext)
        {
            _chatContext = chatContext;
        }

        public IAsyncEnumerable<Chat> Chats => _chatContext.Chat.ToAsyncEnumerable();

        public IEnumerable<Chat> GetUserChats(AppUser user)
        {
            var result =_chatContext.Chat.Where(p => p.IdentityId == user.Id);
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ChatApp.Models.Context;
using ChatApp.Models.Entities;
using ChatApp.Models.Identity;
using ChatApp.ViewModels;

namespace ChatApp.Services.FriendService
{
    public class FriendService : IFriendService
    {
        private readonly ChatContext _chatContext;
        private readonly IUserService _userService;
       

        public FriendService(ChatContext chatContext, IUserService userService)
        {
            _chatContext = chatContext;
            _userService = userService;
        }

       

        public async Task<IEnumerable<Friends>> GetFriends(AppUser user)
        {

            var result = _chatContext.Friends
                .Where(p => p.IdentityId == user.Id);
            
                
          
            return result;
        }
    }
}

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

       

        public async Task<IDictionary<string, string>> GetFriends(AppUser user)
        {
          
            var result  = _chatContext.Friends
                .Where(p => p.IdentityId == user.Id)
                .ToDictionary(p => p.Friend.UserName, i => i.FriendId);

            //var result = _chatContext.Friends
            //    .Where(p => p.IdentityId == user.Id);


            //for (int i = 0; i < result.Length; i++)
            //{
            //    var friends = _userService.GetAppUsers.Select(p => p.Id == result[i]).ToString();
            //    test.Add(friends);

            //}



            return result;
        }
    }
}

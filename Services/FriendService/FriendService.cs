using ChatApp.Models.Entities;
using ChatApp.Models.Identity;
using ChatApp.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        private async Task<IDictionary<string, string>> GetFriendsFromDb(IdentityUserVM user) => await Task.Run(() => _chatContext.Friends
                 .Where(p => p.IdentityId == user.Id)
                 .ToDictionary(p => p.Friend.UserName, i => i.FriendId));

        public async Task<FriendsViewModel[]> GetFriends(IdentityUserVM user)
        {
            var friends = await GetFriendsFromDb(user);
            var viewModel = friends.Select(p => new FriendsViewModel
            {
                AmountOfFriends = friends.Count(),
                Name = p.Key,
                IdentityId = p.Value
            }).ToArray();
            return viewModel;
        }
    }
}
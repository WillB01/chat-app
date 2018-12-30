using ChatApp.Models.Entities;
using ChatApp.ViewModels;
using Microsoft.EntityFrameworkCore;
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

        private async Task<IDictionary<string, string>> GetFriendsFromDb(IdentityUserVM user)
        {
            var resut = await _chatContext.Friends
                 .Include(e => e.Friend)
                 .Where(p => p.IdentityId == user.Id)
                 .ToDictionaryAsync(p => p.Friend.UserName, i => i.FriendId);

            return resut;
        }

        public async Task<FriendsVM[]> GetFriends(IdentityUserVM user)
        {
            var friends = await GetFriendsFromDb(user);
            var viewModel = friends.Select(p => new FriendsVM
            {
                AmountOfFriends = friends.Count(),
                Name = p.Key,
                IdentityId = p.Value
            }).ToArray();
            return viewModel;
        }

        public async Task AddNewFriend(FriendRequestVM friendRequest)
        {
            var dbModelFirstFriend = new Friends
            {
                IdentityId = friendRequest.FromUser,
                FriendId = friendRequest.ToUser,
            };
            var dbModelSecondFriend = new Friends
            {
                IdentityId = friendRequest.ToUser,
                FriendId = friendRequest.FromUser,
            };

            _chatContext.Friends.Add(dbModelFirstFriend);
            _chatContext.Friends.Add(dbModelSecondFriend);

            await _chatContext.SaveChangesAsync();
        }
    }
}
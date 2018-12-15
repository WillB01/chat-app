using ChatApp.Models.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatApp.Services.FriendService
{
    public interface IFriendService
    {
        Task<IDictionary<string, string>> GetFriends(AppUser user);
    }
}
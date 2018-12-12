using ChatApp.Models.Entities;
using ChatApp.Models.Identity;
using ChatApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChatApp.Services.FriendService
{
    public interface IFriendService
    {
        Task<IEnumerable<Friends>> GetFriends(AppUser user);
    }
}

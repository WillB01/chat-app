﻿
using ChatApp.Services;
using ChatApp.Services.FriendRequestService;
using ChatApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Controllers.Search
{
    public class SearchController : Controller
    {
        private readonly IUserService _userService;
        private readonly IFriendRequestService _friendRequestService;
       
        public SearchController(IUserService userService, IFriendRequestService friendRequestService)
        {
            _userService = userService;
            _friendRequestService = friendRequestService;
        }

        [HttpGet]
        public async Task<TypeaheadUsersVM[]> Friends([FromQuery] string q)
        {
            var queryString = q.ToLower();
            var users = await _userService.GetAppUsers();
            var viewModel = users.Where(u => u.UserName.ToLower().StartsWith(queryString))
                .Select(p =>  new TypeaheadUsersVM
            {
                UserName = p.UserName,
                Id = p.Id
            }).ToArray();
            
            return viewModel;
        }

        [HttpPost]
        public async Task PostSearchResult([FromBody] TypeaheadUsersVM person)
        {
            var loggedinUser = await _userService.GetloggedinUser();

            var friendRequestVM = new FriendRequestVM
            {
                FromUser = loggedinUser.Id.ToString(),
                ToUser = person.Id,
                HasAccepted = false
            };

            await _friendRequestService.SendFriendRequest(friendRequestVM);
        }
    }
}

using ChatApp.Models.Identity;
using ChatApp.Services;
using ChatApp.Services.FriendService;
using ChatApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Controllers.Home
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly IChatService _chatService;
        private readonly IFriendService _friendService;

        public HomeController(IUserService userService, IChatService chatService, IFriendService friendService)
        {
            _userService = userService;
            _chatService = chatService;
            _friendService = friendService;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var loggedinUser = HttpContext.User;
            var user = await _userService.GetloggedinUser(loggedinUser);
            var chats =   _chatService.GetUserChats(user);
            var friends = await _friendService.GetFriends(user);
            var test = friends.Select(p => p.Friend.UserName);
            
            return View(chats);
        }


    }
}

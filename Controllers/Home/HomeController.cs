using ChatApp.Models.Identity;
using ChatApp.Services;
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

        public HomeController(IUserService userService, IChatService chatService)
        {
            _userService = userService;
            _chatService = chatService;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var loggedinUser = HttpContext.User;
            var user = await _userService.GetloggedinUser(loggedinUser);
            var chats =  _chatService.GetUserChats(user);
            return View(chats);
        }


    }
}

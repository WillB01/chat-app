using ChatApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Controllers.Friends
{
    public class FriendsController : Controller
    {
        private readonly IUserService _userService;

        public FriendsController(IUserService userService)
        {
            _userService = userService;
        }
    }
}
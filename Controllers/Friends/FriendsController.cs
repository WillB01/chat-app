using ChatApp.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

using ChatApp.Models.Identity;
using ChatApp.Services;
using Microsoft.AspNetCore.Authorization;
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

        public HomeController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            //var u = new User()
            //{
            //    Email = "bb@test.com",
            //    Name = "John",
            //    Password = "hejhej"
            //};
            //var result = await _userService.CreateUserAsync(u);
            return View();
        }

      


    }
}

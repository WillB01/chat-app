using ChatApp.Controllers.Home;
using ChatApp.Models.Identity;
using ChatApp.Services;
using ChatApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Controllers.Account
{
    public class AuthController : Controller
    {
        private readonly IUserService _userService;

        private const string INDEX = "Index";
        private const string HOME = "Home";


        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(MainViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await _userService.LoginAsync(user);

            if (!result.Succeeded)
            {
                return View();
            }
            return RedirectToAction(INDEX, HOME);
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _userService.LogoutAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Register(MainViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await _userService.CreateUserAsync(user);

            if (!result.Succeeded)
            {
                return RedirectToAction(nameof(Login));
            }
            return RedirectToAction(INDEX, HOME);
        }
    }
}

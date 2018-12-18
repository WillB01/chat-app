using ChatApp.Services;
using ChatApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(MainVM user1)
        {
            
            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await _userService.LoginAsync(user1);

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
        public async Task<IActionResult> Register(MainVM user1)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await _userService.CreateUserAsync(user1);

            if (!result.Succeeded)
            {
                return RedirectToAction(nameof(Login));
            }
            return RedirectToAction(INDEX, HOME);
        }
    }
}
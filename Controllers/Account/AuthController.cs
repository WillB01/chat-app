using ChatApp.Services.AuthService;
using ChatApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ChatApp.Controllers.Account
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        private const string INDEX = "Index";
        private const string HOME = "Home";

        public AuthController(IAuthService authService)
        {
            _authService = authService;
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

            var result = await _authService.LoginAsync(user1);

            if (!result.Succeeded)
            {
                return View();
            }
            return RedirectToAction(INDEX, HOME);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Register(MainVM user1)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await _authService.CreateUserAsync(user1);

            if (!result.Succeeded)
            {
                return RedirectToAction(nameof(Login));
            }
            return RedirectToAction(INDEX, HOME);
        }
    }
}
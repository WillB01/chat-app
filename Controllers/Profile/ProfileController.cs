using ChatApp.Services;
using ChatApp.Services.FriendService;
using ChatApp.Services.ProfileService;
using ChatApp.Services.ViewModelService;
using ChatApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace ChatApp.Controllers.Profile
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IUserService _userService;
        private readonly IProfileImageService _profileImageService;
        private readonly IViewModelService _viewModelService;
        private readonly IFriendService _friendService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ProfileController(IUserService userService, IViewModelService viewModelService, IProfileImageService profileImageService,
            IFriendService friendService, IHostingEnvironment hostingEnvironment)
        {
            _userService = userService;
            _viewModelService = viewModelService;
            _profileImageService = profileImageService;
            _friendService = friendService;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(new AddProfileImageVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AddProfileImageVM AddProfileImageVM)
        {
            //ViewData["ReturnUrl"] = returnUrl;
            var user = await _userService.GetloggedinUser();
            var profileImage = new ProfileImageVM
            {
                UserId = user.Id
            };
            if (ModelState.IsValid)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await AddProfileImageVM.ProfileImage.CopyToAsync(memoryStream);
                    profileImage.ProfileImage1 = memoryStream.ToArray();
                    await _profileImageService.AddProfileImage(profileImage);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GetImage()
        {
            //if (await _profileImageService.GetProfileImage() == null)
            //{
            //    var webRootPath = _hostingEnvironment.WebRootPath + "/img/test3.png";
            //    var bytes = System.IO.File.ReadAllBytes(webRootPath);
            //    return null;

            //}
            //return File( await _profileImageService.GetProfileImage(), "image/*");
            var userName = await _userService.GetloggedinUser();
            return Ok(Json(new { img = await _profileImageService.GetProfileImage(), userName = userName.UserName }));
        }

        public async Task<IActionResult> GetFreindsProfileImages()
        {
            return Ok(Json(await _profileImageService.GetFriendsProfileImagesAsync()));
        }
    }
}
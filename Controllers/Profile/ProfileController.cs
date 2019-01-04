using ChatApp.Services;
using ChatApp.Services.FriendService;
using ChatApp.Services.ProfileService;
using ChatApp.Services.ViewModelService;
using ChatApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public ProfileController(IUserService userService, IViewModelService viewModelService, IProfileImageService profileImageService, IFriendService friendService)
        {
            _userService = userService;
            _viewModelService = viewModelService;
            _profileImageService = profileImageService;
            _friendService = friendService;
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
            return File(await _profileImageService.GetProfileImage(), "image/*");
            
        }

        public async Task GetFreindsProfileImages()
        {
           
            await _profileImageService.GetFriendsProfileImagesAsync();
        }
    }
}

using ChatApp.Services;
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

        public ProfileController(IUserService userService, IViewModelService viewModelService, IProfileImageService profileImageService)
        {
            _userService = userService;
            _viewModelService = viewModelService;
            _profileImageService = profileImageService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            _viewModelService.IdentityUserVM = await _userService.GetloggedinUser();
            return View(_viewModelService);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> UploadProfileImage(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new { count = files.Count, size, filePath });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task Register(AddProfileImageVM model)
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
                    await model.AvatarImage.CopyToAsync(memoryStream);
                    profileImage.ProfileImage1 = memoryStream.ToArray();
                    //_profileImageService.
                    
                }
                // additional logic omitted

                // Don't rely on or trust the model.AvatarImage.FileName property 
                // without validation.
            }
        }
    }
}

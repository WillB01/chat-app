using ChatApp.Models.Entities;
using ChatApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Services.ProfileService
{
    public class ProfileImageService : IProfileImageService
    {
        private readonly ChatContext _chatContext;
        private readonly IUserService _userService;
        public ProfileImageService(ChatContext chatContext, IUserService userService)
        {
            _chatContext = chatContext;
            _userService = userService;
        }

        public async Task SaveProfileImageAsync(ProfileImageVM profileImageVM)
        {
            var dbModel = new ProfileImage
            {
                ProfileImage1 = profileImageVM.ProfileImage1,
                UserId = profileImageVM.UserId
            };

            _chatContext.ProfileImage.Add(dbModel);
            await _chatContext.SaveChangesAsync();
        }

        public async Task<ProfileImageVM> GetProfileImage()
        {
            var user = await _userService.GetloggedinUser();
            var profileImage = await _chatContext.ProfileImage
                .Where(p => p.UserId == user.Id)
                .Select(e => new ProfileImageVM
                {
                    ProfileImage1 = e.ProfileImage1,
                    UserId = e.UserId
                })
                .FirstOrDefaultAsync();
            return profileImage;
        }
    }
}

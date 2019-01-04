using ChatApp.Models.Entities;
using ChatApp.Services.FriendService;
using ChatApp.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Services.ProfileService
{
    public class ProfileImageService : IProfileImageService
    {
        private readonly ChatContext _chatContext;
        private readonly IUserService _userService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IFriendService _friendService;


        public ProfileImageService(ChatContext chatContext, IUserService userService, IHostingEnvironment hostingEnvironment, IFriendService friendService)
        {
            _chatContext = chatContext;
            _userService = userService;
            _hostingEnvironment = hostingEnvironment;
            _friendService = friendService;
        }

        private async Task SaveProfileImageAsync(ProfileImageVM profileImageVM)
        {
            var dbModel = new ProfileImage
            {
                ProfileImage1 = profileImageVM.ProfileImage1,
                UserId = profileImageVM.UserId
            };

            _chatContext.ProfileImage.Add(dbModel);
            await _chatContext.SaveChangesAsync();
        }

        public async Task<byte[]> GetProfileImage()
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
            if (profileImage == null)
            {
                var webRootPath = _hostingEnvironment.WebRootPath + "/img/test.png";
                var bytes = File.ReadAllBytes(webRootPath);
                return bytes;
            }
          
            return profileImage.ProfileImage1;
        }

        public async Task AddProfileImage(ProfileImageVM profileImageVM)
        {
            var dbModel = new ProfileImage
            {
                ProfileImage1 = profileImageVM.ProfileImage1,
                UserId = profileImageVM.UserId
            };
            if (await GetProfileImage() == null)
            {
                await SaveProfileImageAsync(profileImageVM);
            }else
            {
                var update = await _chatContext.ProfileImage
                    .Where(e => e.UserId == profileImageVM.UserId)
                    .FirstOrDefaultAsync();

                update.ProfileImage1 = profileImageVM.ProfileImage1;

                await _chatContext.SaveChangesAsync();
            }
              

        }

        public async Task GetFriendsProfileImagesAsync()
        {
            var friends = await _friendService.GetFriends(await _userService.GetloggedinUser());
            var profileImageVMs = new List<ProfileImageVM>();
            foreach (var friend in friends)
            {
                profileImageVMs.Add(await _chatContext.ProfileImage
                    .Where(p => p.UserId == friend.IdentityId)
                    .Select(e => new ProfileImageVM {
                        ProfileImage1 = e.ProfileImage1,
                        UserId = e.UserId
                    })
                    .FirstOrDefaultAsync());
            }

            var test = profileImageVMs; //TODO FIIIIIIIX

        }
    }
}

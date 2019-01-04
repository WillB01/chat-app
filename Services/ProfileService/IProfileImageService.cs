using ChatApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Services.ProfileService
{
    public interface IProfileImageService
    {
        Task SaveProfileImageAsync(ProfileImageVM profileImageVM);
        Task<ProfileImageVM> GetProfileImage();
    }
}

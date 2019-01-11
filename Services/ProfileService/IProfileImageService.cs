using ChatApp.ViewModels;
using System.Threading.Tasks;

namespace ChatApp.Services.ProfileService
{
    public interface IProfileImageService
    {
        Task<byte[]> GetProfileImage();

        Task AddProfileImage(ProfileImageVM profileImageVM);

        Task<ProfileImageVM[]> GetFriendsProfileImagesAsync();
    }
}
using ChatApp.ViewModels;
using System.Threading.Tasks;

namespace ChatApp.Services
{
    public interface IUserService
    {
        Task<IdentityUserVM[]> GetAppUsers();

        Task<IdentityUserVM> GetloggedinUser();

        Task<IdentityUserVM> GetUserByUserName(string name);

        Task<string> GetUserId(string name);

        Task<string> GetUserNameById(string id);
    }
}
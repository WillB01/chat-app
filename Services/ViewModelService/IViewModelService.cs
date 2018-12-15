using ChatApp.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatApp.Services.ViewModelService
{
    public interface IViewModelService
    {
        LoginUserViewModel UserLoginViewModel { get; set; }
        RegisterNewUserViewModel RegisterNewUserView { get; set; }
        Task<IEnumerable<ChatsViewModel>> ChatsViewModels { get; set; }
        IDictionary<string, string> FreindsViewModel { get; set; }
        IdentityUserVM User { get; set; }
    }
}
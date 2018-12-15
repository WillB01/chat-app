using ChatApp.Services.ViewModelService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatApp.ViewModels
{
    public class MainViewModel : IViewModelService
    {
        public LoginUserViewModel UserLoginViewModel { get; set; }

        public RegisterNewUserViewModel RegisterNewUserView { get; set; }

        public Task<IEnumerable<ChatsViewModel>> ChatsViewModels { get; set; }

        public IDictionary<string, string> FreindsViewModel { get; set; }

        public IdentityUserVM User { get; set; }
    }
}
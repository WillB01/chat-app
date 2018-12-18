using ChatApp.Services.ViewModelService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatApp.ViewModels
{
    public class MainVM : IViewModelService
    {
        public LoginUserVM UserLoginVM { get; set; }

        public RegisterNewUserVM RegisterNewVM { get; set; }

        public Task<IEnumerable<MessageVM>> MessageVM { get; set; }

        public FriendsVM[] FriendsVM { get; set; }

        public IdentityUserVM[] IdentityUsersVM { get; set; }

        public TypeaheadUsersVM[] TypeaheadUsersVM { get; set; }

        public FriendRequestVM[] FriendRequestVM { get; set; }
        public IdentityUserVM IdentityUserVM { get; set; }
    }
}
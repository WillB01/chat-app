using ChatApp.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatApp.Services.ViewModelService
{
    public interface IViewModelService
    {
        LoginUserVM UserLoginVM { get; set; }
        RegisterNewUserVM RegisterNewVM { get; set; }
        Task<IEnumerable<MessageVM>> MessageVM { get; set; }
        FriendsVM[] FriendsVM { get; set; }
        IdentityUserVM[] IdentityUserVM { get; set; }
        TypeaheadUsersVM[] TypeaheadUsersVM { get; set; }
        FriendRequestVM[] FriendRequestVM { get; set; }
    }
}
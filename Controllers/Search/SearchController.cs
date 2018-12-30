using ChatApp.Hubs.FriendRequestHub;
using ChatApp.Services;
using ChatApp.Services.FriendRequestService;
using ChatApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Controllers.Search
{
    public class SearchController : Controller
    {
        private readonly IUserService _userService;
        private readonly IFriendRequestService _friendRequestService;
        private readonly IHubContext<FriendRequestHub, IFriendClient> _friendRequestHubContext;

        public SearchController(IUserService userService, IFriendRequestService friendRequestService, IHubContext<FriendRequestHub, IFriendClient> friendRequestHubContext)
        {
            _userService = userService;
            _friendRequestService = friendRequestService;
            _friendRequestHubContext = friendRequestHubContext;
        }

        [HttpGet]
        public async Task<TypeaheadUsersVM[]> Friends([FromQuery] string q)
        {
            var loggedinUser = await _userService.GetloggedinUser();
            var queryString = q.ToLower();
            var users = await _userService.GetAppUsers();
            var viewModel = users.Where(u => u.UserName.ToLower().StartsWith(queryString) && u.UserName != loggedinUser.UserName)
                .Select(p => new TypeaheadUsersVM
                {
                    UserName = p.UserName,
                    Id = p.Id
                }).ToArray();

            return viewModel;
        }

        [HttpPost]
        public async Task PostSearchResult([FromBody] TypeaheadUsersVM person)
        {
            var loggedinUser = await _userService.GetloggedinUser();

            var friendRequestVM = new FriendRequestVM
            {
                FromUser = loggedinUser.Id,
                ToUser = person.Id,
                FromUserName = loggedinUser.UserName,
                ToUserName = person.UserName
            };

            await _friendRequestService.SendFriendRequest(friendRequestVM);
            var signalRModel = new FriendRequestVM[] { friendRequestVM };
            await _friendRequestHubContext.Clients.User(friendRequestVM.ToUser)
                .ReceiveFriendRequest(true, signalRModel);

        }
    }
}
using ChatApp.Services;
using ChatApp.Services.FriendRequestService;
using ChatApp.Services.FriendService;
using ChatApp.Services.ViewModelService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ChatApp.Controllers.Home
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly IChatService _chatService;
        private readonly IFriendService _friendService;
        private readonly IViewModelService _viewModelService;
        private readonly IFriendRequestService _friendRequestService;

        public HomeController(IUserService userService, IChatService chatService, 
            IFriendService friendService, IViewModelService viewModelService, IFriendRequestService friendRequestService)
        {
            _userService = userService;
            _chatService = chatService;
            _friendService = friendService;
            _viewModelService = viewModelService;
            _friendRequestService = friendRequestService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var loggedinUser = HttpContext.User;
            var user = await _userService.GetloggedinUser(loggedinUser);
            var friends = await _friendService.GetFriends(user);
            var friendRequests = await _friendRequestService.CheckFriendRequest(user);

            _viewModelService.FriendsVM = friends;
            _viewModelService.FriendRequestVM = friendRequests;

            return View(_viewModelService);
        }


    }
}
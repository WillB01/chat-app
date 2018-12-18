using ChatApp.Services;
using ChatApp.Services.ViewModelService;
using ChatApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ChatApp.Controllers.Friends
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IUserService _userService;
        private readonly IViewModelService _viewModelService;

        public ProfileController(IUserService userService, IViewModelService viewModelService)
        {
            _userService = userService;
            _viewModelService = viewModelService;
        }

        //[HttpGet]
        //public IActionResult Index(IdentityUserVM user)
        //{
        //    return View(user);
        //}

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] TypeaheadUsersVM person)
        {

            //var userName = await Task.Run(() => HttpContext.User.Identity.Name);
            //var userId = await _userService.GetUserId(userName);

            var user = await _userService.GetUserByUserName(person.UserName);

           
            //var ToUser = await _userService.GetUserNameById(person.Id);

            //var friendRequestVM = new FriendRequestVM
            //{
            //    FromUser = userId,
            //    ToUser = person.Id,
            //    HasAccepted = false,
            //    FromUserName = userName
                
            //};

            _viewModelService.IdentityUserVM = user;

            //await _friendRequestService.SendFriendRequest(friendRequestVM);

            return View(user);

        }

        //[HttpPost]
        //public async Task<IActionResult> Index([FromBody] TypeaheadUsersVM person)
        //{

        //    var userName = await Task.Run(() => HttpContext.User.Identity.Name);
        //    var userId = await _userService.GetUserId(userName);


        //    //var ToUser = await _userService.GetUserNameById(person.Id);

        //    var friendRequestVM = new FriendRequestVM
        //    {
        //        FromUser = userId,
        //        ToUser = person.Id,
        //        HasAccepted = false,
        //        FromUserName = userName

        //    };

        //    _viewModelService.FriendRequestVM[0] = friendRequestVM;

        //    //await _friendRequestService.SendFriendRequest(friendRequestVM);

        //    return View(_viewModelService);

        //}
    }
}
using ChatApp.Services;
using ChatApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Controllers.Search
{
    public class SearchController : Controller
    {
        private readonly IUserService _userService;

        public SearchController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<TypeaheadUsersVM[]>  Friends([FromQuery] string q)
        {
            var queryString = q.ToLower();
            var users = await _userService.GetAppUsers();
            var viewModel = users.Where(u => u.UserName.ToLower().StartsWith(queryString))
                .Select(p =>  new TypeaheadUsersVM
            {
                UserName = p.UserName,
                Id = p.Id
            }).ToArray();
            
            return viewModel;
        }

        [HttpPost]
        public async Task PostSearchResult([FromBody] TypeaheadUsersVM person)
        {
            var b = person;
        }
    }
}

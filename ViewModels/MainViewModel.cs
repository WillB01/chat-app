using ChatApp.Models.Entities;
using ChatApp.Services.ViewModelService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.ViewModels
{
    public class MainViewModel : IViewModelService
    {
        

        public LoginUserViewModel UserLoginViewModel { get; set; }
        public RegisterNewUserViewModel RegisterNewUserView { get; set; }
        public Task<IEnumerable<Chat>> ChatsViewModels { get; set; }

        public IDictionary<string, string> FreindsViewModel { get; set; }
    }
}

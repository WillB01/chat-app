using ChatApp.Models.Entities;
using ChatApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Services.ViewModelService
{
    public interface IViewModelService
    {
        LoginUserViewModel UserLoginViewModel { get; set; }
        RegisterNewUserViewModel RegisterNewUserView { get; set; }
        IEnumerable<Chat> ChatsViewModels { get; set; }
        IDictionary<string, string> FreindsViewModel { get; set; }
    }
}

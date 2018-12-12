using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.ViewModels
{
    public class MainViewModel
    {
        public LoginUserViewModel UserLoginViewModel { get; set; }
        public RegisterNewUserViewModel RegisterNewUserView { get; set; }
        public IEnumerable<ChatsViewModel> ChatsViewModels { get; set; }
    }
}

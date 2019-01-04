using ChatApp.Services.ProfileService;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.ViewModels
{
    public class AddProfileImageVM
    {
        [Required]
        public IFormFile AvatarImage { get; set; }
    }
}

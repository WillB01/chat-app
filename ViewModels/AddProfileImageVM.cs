using ChatApp.Services.ProfileService;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.ViewModels
{
    public class AddProfileImageVM
    {
        [Display(Name = "Profile Image")]
        [Required]
        public IFormFile ProfileImage { get; set; }
    }
}

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ChatApp.ViewModels
{
    public class AddProfileImageVM
    {
        [Display(Name = "Profile Image")]
        [Required]
        public IFormFile ProfileImage { get; set; }
    }
}
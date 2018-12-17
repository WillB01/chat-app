using System.ComponentModel.DataAnnotations;

namespace ChatApp.ViewModels
{
    public class LoginUserVM
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
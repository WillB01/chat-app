using System.ComponentModel.DataAnnotations;

namespace ChatApp.ViewModels
{
    public class RegisterNewUserViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace AROBlog.Models.UserViewModels
{
    public class RegisterViewModel
    {

        [Required]
        public string Account { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}

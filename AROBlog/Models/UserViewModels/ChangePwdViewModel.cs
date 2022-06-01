using System.ComponentModel.DataAnnotations;

namespace AROBlog.Models.UserViewModels
{
    public class ChangePwdViewModel
    {
        [Required]
        public string Account { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 6)]
        public string OldPwd { get; set; }

        [Required]
        public string NewPwd { get; set; }
    }
}

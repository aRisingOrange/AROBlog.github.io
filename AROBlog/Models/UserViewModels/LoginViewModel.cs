using System.ComponentModel.DataAnnotations;

namespace AROBlog.Models.UserViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "账号")]
        public string LoginAccount { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 6)]
        [Display(Name = "密码")]
        [DataType(dataType: DataType.Password)]
        public string LoginPwd { get; set; }
        [Display(Name = "记住我")]
        public bool RememberMe { get; set; }
    }
}

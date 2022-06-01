using System.ComponentModel.DataAnnotations;

namespace AROBlog.Models.BlogViewModels
{
    public class CreateCategoryViewModel
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string CategoryName { get; set; }

    }
}

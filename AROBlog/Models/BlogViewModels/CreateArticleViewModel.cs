using System.ComponentModel.DataAnnotations;

namespace AROBlog.Models.BlogViewModels
{
    public class CreateArticleViewModel
    {
    
        //public string? Title { get; set; }

        //public string? Path { get; set; }
        [Required]
        public Guid[] CategoryIds { get; set; }
        [Required]
        public string? Summary { get; set; }
    }
}

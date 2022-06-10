using AROBlog.DTO;

namespace AROBlog.Models.BlogViewModels
{
    public class ArticleViewModel
    {
        public List<ArticleDTO> articleDTOs { get; set; }
        public List<CategoryDTO> categoryDTOs { get; set; }
        public Guid categoryId { get; set; }
    }
}

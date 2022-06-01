namespace AROBlog.Models.BlogViewModels
{
    public class ArticleDetailsViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string StoragePath { get; set; }

        public DateTime CreateTime { get; set; }

        public string[] CategoryIds { get; set; }

        public string[] CategoryNames { get; set; }
    }
}

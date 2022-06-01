using AROBlog.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace AROBlog.DTO
{
    public class ArticleDTO
    {
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 文章的存放位置
        /// </summary>
        public string StoragePath { get; set; }
        public string[] CategoryNames { get; set; }
        public Guid[] CategoryIds { get; set; }

    }
}
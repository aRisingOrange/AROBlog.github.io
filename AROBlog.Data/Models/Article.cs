using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AROBlog.Data.Models
{
    public class Article : BaseEntity
    {
        [Required]
        public string Title { get; set; }

        public string? Summary { get; set; }
        /// <summary>
        /// 文章的存放位置
        /// </summary>
        public string StoragePath { get; set; }

        //[ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public User User { get; set; }

        public List<ArticleToCategory> ArticleToCategories { get; set; }

    }
}

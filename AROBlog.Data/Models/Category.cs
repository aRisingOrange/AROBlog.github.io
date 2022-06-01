using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AROBlog.Data.Models
{
    public class Category:BaseEntity
    {
        public string CategoryName { get; set; }


        public Guid UserId { get; set; }
        public User User { get; set; }

        public List<ArticleToCategory> ArticleToCategories { get; set; }

    }
}

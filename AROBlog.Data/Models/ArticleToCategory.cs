using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AROBlog.Data.Models
{
    public class ArticleToCategory:BaseEntity
    {
    
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }


        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ArticleId { get; set; }
        public Article Article { get; set; }
    }
}

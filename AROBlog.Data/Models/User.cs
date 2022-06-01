using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AROBlog.Data.Models
{
    public class User:BaseEntity
    {
        [Required, StringLength(30), Column(TypeName = "varchar")]
        public string Account { get; set; }


        [Required,StringLength(30),Column(TypeName ="varchar")]
        public string Password { get; set; }

        public List<Article> Articles { get; set; }
        public List<Category> Categories { get; set; } 

    }
}

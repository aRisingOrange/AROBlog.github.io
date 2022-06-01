using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AROBlog.Data.Models
{
    public class Photo:BaseEntity
    {
        public string Title { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 照片存放位置
        /// </summary>
        public string StoragePath {  get; set; }
        /// <summary>
        /// 高度
        /// </summary>
        public long Height { get; set; }

        /// <summary>
        /// 宽度
        /// </summary>
        public long Width { get; set; }
    }
}

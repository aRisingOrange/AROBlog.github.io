using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AROBlog.Data.Models
{
    public  class BaseEntity
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 是否被删除（伪删除）
        /// </summary>
        public bool IsRemoved { get; set; }

        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}

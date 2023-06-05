using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.DomainClasses
{
    public class News: DomainClassBase
    {
        public News()
        {
            CreateDate = DateTimeOffset.UtcNow;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTimeOffset CreateDate { get; set; }
    }
}

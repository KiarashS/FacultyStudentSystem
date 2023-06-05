using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.Models.ViewModels
{
    public class NewsFeedViewModel
    {
        public NewsFeedViewModel()
        {
            AuthorName = "مدیر سیستم";
        }

        public int Id { set; get; }
        public string Title { set; get; }
        public string AuthorName { set; get; }
        public string Body { set; get; }
        public DateTimeOffset Date { set; get; }
    }
}

﻿using System;

namespace ContentManagementSystem.Commons.Web.MvcRss
{
    public class FeedItem
    {
        public string Title { set; get; }
        public string AuthorName { set; get; }
        public string Content { set; get; }
        public string Url { set; get; }
        public DateTime LastUpdatedTime { set; get; }
        public DateTime PublishDate { set; get; }
    }
}

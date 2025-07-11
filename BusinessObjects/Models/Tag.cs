﻿using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Tag
{
    public int TagId { get; set; }

    public string? TagName { get; set; }

    public string? Note { get; set; }

    public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
    //public virtual ICollection<NewsTag> NewsTags { get; set; } = new List<NewsTag>();

}

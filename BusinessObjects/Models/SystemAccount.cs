﻿using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class SystemAccount
{
    public short AccountId { get; set; }

    public string? AccountName { get; set; }

    public string? AccountEmail { get; set; }

    public int? AccountRole { get; set; }

    public string? AccountPassword { get; set; }

    public string? AuthenticatorKey { get; set; }

    public bool? IsMfaEnabled { get; set; } = false;

    public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
}

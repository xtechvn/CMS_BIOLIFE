using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Tag
{
    public long Id { get; set; }

    public string TagName { get; set; }

    public DateTime? CreatedOn { get; set; }

    public virtual ICollection<ArticleTag> ArticleTags { get; set; } = new List<ArticleTag>();
}

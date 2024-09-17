using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class ArticleCategory
{
    public long Id { get; set; }

    public int? CategoryId { get; set; }

    public long? ArticleId { get; set; }

    public DateTime? UpdateLast { get; set; }
}

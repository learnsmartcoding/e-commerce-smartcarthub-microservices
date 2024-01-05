using System;
using System.Collections.Generic;

namespace User.Core.Entities;

public partial class ProductReview
{
    public int ReviewId { get; set; }

    public int? ProductId { get; set; }

    public int? UserId { get; set; }

    public int Rating { get; set; }

    public string? ReviewText { get; set; }

    public DateTime ReviewDate { get; set; }

    public virtual Product? Product { get; set; }

    public virtual UserProfile? User { get; set; }
}

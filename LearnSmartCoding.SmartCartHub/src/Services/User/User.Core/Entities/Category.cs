using System;
using System.Collections.Generic;

namespace User.Core.Entities;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Coupon> Coupons { get; set; } = new List<Coupon>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}

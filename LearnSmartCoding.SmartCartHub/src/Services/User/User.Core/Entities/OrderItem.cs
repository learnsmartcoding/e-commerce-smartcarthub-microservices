using System;
using System.Collections.Generic;

namespace User.Core.Entities;

public partial class OrderItem
{
    public int OrderItemId { get; set; }

    public int? OrderId { get; set; }

    public int? ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public decimal TotalCost { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }
}

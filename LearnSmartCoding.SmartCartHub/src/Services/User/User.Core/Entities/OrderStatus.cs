using System;
using System.Collections.Generic;

namespace User.Core.Entities;

public partial class OrderStatus
{
    public int StatusId { get; set; }

    public int? OrderId { get; set; }

    public string StatusName { get; set; } = null!;

    public virtual Order? Order { get; set; }
}

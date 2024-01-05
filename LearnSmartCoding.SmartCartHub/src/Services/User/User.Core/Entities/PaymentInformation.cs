using System;
using System.Collections.Generic;

namespace User.Core.Entities;

public partial class PaymentInformation
{
    public int PaymentId { get; set; }

    public int? OrderId { get; set; }

    public decimal PaymentAmount { get; set; }

    public DateTime PaymentDate { get; set; }

    public string? PaymentMethod { get; set; }

    public virtual Order? Order { get; set; }
}

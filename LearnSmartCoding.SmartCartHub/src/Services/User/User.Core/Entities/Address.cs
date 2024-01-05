using System;
using System.Collections.Generic;

namespace User.Core.Entities;

public partial class Address
{
    public int AddressId { get; set; }

    public int? UserId { get; set; }

    public string Street { get; set; } = null!;

    public string City { get; set; } = null!;

    public string State { get; set; } = null!;

    public string ZipCode { get; set; } = null!;

    public bool IsShippingAddress { get; set; }

    public virtual UserProfile? User { get; set; }
}

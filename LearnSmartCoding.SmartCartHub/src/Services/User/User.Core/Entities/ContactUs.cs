using System;
using System.Collections.Generic;

namespace User.Core.Entities;

public partial class ContactUs
{
    public int ContactUsId { get; set; }

    public string UserName { get; set; } = null!;
    public string UserEmail { get; set; } = null!;
    public string MessageDetail { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace User.Core.Entities;

public partial class UserActivityLog
{
    public int LogId { get; set; }

    public int? UserId { get; set; }

    public string ActivityType { get; set; } = null!;

    public string? ActivityDescription { get; set; }

    public DateTime LogDate { get; set; }

    public virtual UserProfile? User { get; set; }
}

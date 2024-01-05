using System;
using System.Collections.Generic;

namespace User.Core.Entities;

public partial class UserRole
{
    public int UserRoleId { get; set; }

    public int? RoleId { get; set; }

    public int? UserId { get; set; }

    public virtual Role? Role { get; set; }

    public virtual UserProfile? User { get; set; }
}

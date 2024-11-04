using System;
using System.Collections.Generic;

namespace MusicStore.Models;

public partial class Membership
{
    public int MembershipId { get; set; }

    public int? UserId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public string Status { get; set; } = null!;

    public virtual User? User { get; set; }
}

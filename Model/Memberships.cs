using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Memberships
    {
        public int MembershipId;
        public virtual User User { get; set; } 
        public DateTime StartDate;
        public DateTime EndDate;
        public string Status;

        public Memberships() { }

        public Memberships(int membershipId, User user, DateTime startDate, DateTime endDate, string status)
        {
            MembershipId = membershipId;
            User = user;
            StartDate = startDate;
            EndDate = endDate;
            Status = status;
        }
    }
}

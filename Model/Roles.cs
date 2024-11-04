using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Roles
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class User
    {
        public int UserId;
        public virtual Roles Role { get; set; }
        public string UserName;
        public string Password;
        public string Email;
        public ICollection<Orders> Orders { get; set; } = new List<Orders>();
        public ICollection<Memberships> Memberships { get; set; }

        public User() 
        {
        }

        public User(int userId, Roles role, string userName, string password, string email)
        {
            UserId = userId;
            Role = role;
            UserName = userName;
            Password = password;
            Email = email;
        }
    }
}

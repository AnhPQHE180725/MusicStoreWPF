using MusicStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.DataAccessLayer
{
    public class UserDAO
    {
        public static User getUserbyUsernamePassword(string username, string password)
        {
            var db = new MusicStoreContext();
            return db.Users.FirstOrDefault(c=>c.Username.Equals(username)&& c.Password.Equals(password));   

        }
    }
}

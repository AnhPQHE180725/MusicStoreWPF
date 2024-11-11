using Microsoft.EntityFrameworkCore;
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
        private static User _loggedInUser = null;

        public static User GetLoggedInUser()
        {
            return _loggedInUser;  // Trả về đối tượng User của người dùng hiện tại
        }
        public static void LogoutUser()
        {
            _loggedInUser = null;
        }
        public static User getUserbyUsernamePassword(string username, string password)
        {
            using (var context = new MusicStoreContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
                if (user != null)
                {
                    _loggedInUser = user; // Lưu thông tin người dùng đã đăng nhập vào biến tạm
                }
                return user;
            }

        }

    }
}

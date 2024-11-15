﻿using Microsoft.EntityFrameworkCore;
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

        public static bool AddUser(User user)
        {
            using (var context = new MusicStoreContext())
            {
                // Kiểm tra xem tên người dùng đã tồn tại chưa
                if (context.Users.Any(u => u.Username == user.Username))
                {
                    return false; // Trả về false nếu tên người dùng đã tồn tại
                }

                try
                {
                    // Lấy giá trị UserId lớn nhất hiện có
                    var maxUserId = context.Users.Max(u => (int?)u.UserId) ?? 0;
                    user.UserId = maxUserId + 1; // Tăng UserId lên 1

                    context.Users.Add(user); // Thêm người dùng mới vào cơ sở dữ liệu
                    context.SaveChanges(); // Lưu thay đổi
                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception("Could not save user to database: " + ex.InnerException?.Message);
                }
            }
        }

    }
}

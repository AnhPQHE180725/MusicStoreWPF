using Microsoft.EntityFrameworkCore;
using MusicStore.DataAccessLayer;
using MusicStore.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MusicStore
{
    /// <summary>
    /// Interaction logic for ManageUser.xaml
    /// </summary>
    public partial class ManageUser : Window
    {
        private readonly MusicStoreContext _context;

        public ManageUser()
        {
            InitializeComponent();
            _context = new MusicStoreContext();
            LoadUsers();
        }

        // Phương thức để tải danh sách người dùng vào DataGrid
        private void LoadUsers()
        {
            var users = _context.Users.Include(u => u.Role).ToList();
            ListUsersDataGrid.ItemsSource = users;
        }


        private void AddAdminButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUserName.Text) && !string.IsNullOrEmpty(txtPassword.Text) && !string.IsNullOrEmpty(txtEmail.Text))
            {
                int newUserId = _context.Users.Any() ? _context.Users.Max(a => a.UserId) + 1 : 1;

                var newUser = new User
                {
                    UserId = newUserId,
                    Username = txtUserName.Text,
                    Password = txtPassword.Text,
                    Email = txtEmail.Text,
                    RoleId = 1 
                };

                _context.Users.Add(newUser);
                _context.SaveChanges();
                LoadUsers();
                ClearFields();
            }
            else
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            AdminScreen ad = new AdminScreen();
            ad.Show();
            this.Close();
        }

        private void DeleteAdmin_Click(object sender, RoutedEventArgs e)
        {
            if (ListUsersDataGrid.SelectedItem is User selectedUser)
            {
                if (selectedUser.RoleId == 1)
                {
                    var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa Admin này không?",
                                                        "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (confirmResult == MessageBoxResult.Yes)
                    {
                        _context.Users.Remove(selectedUser);
                        _context.SaveChanges();
                        LoadUsers();
                        ClearFields();
                        MessageBox.Show("Xóa Admin thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Chỉ có thể xóa Admin.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một người dùng để xóa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }
        // Làm sạch các TextBox
        private void ClearFields()
        {
            txtUserName.Clear();
            txtPassword.Clear();
            txtEmail.Clear();
        }



        private void ListUsersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListUsersDataGrid.SelectedItem is User selectedUser)
            {
                txtUserName.Text = selectedUser.Username;
                txtPassword.Text = selectedUser.Password;
                txtEmail.Text = selectedUser.Email;
                txtPassword.Clear();
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadUsers();
            ClearFields();
        }
    }
}

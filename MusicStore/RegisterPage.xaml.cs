using System.Windows;
using MusicStore.DataAccessLayer;
using MusicStore.Models;

namespace MusicStore
{
    public partial class RegisterPage : Window
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtEmail.Text) ||
                    string.IsNullOrWhiteSpace(txtUsername.Text) ||
                    string.IsNullOrWhiteSpace(txtPassword.Password))
                {
                    errorMessage.Text = "Please fill in all fields.";
                    return;
                }

                var newUser = new User
                {
                    Email = txtEmail.Text,
                    Username = txtUsername.Text,
                    Password = txtPassword.Password,
                    RoleId = 2 // Thiết lập RoleId cho người dùng mới
                };

                bool isSuccess = UserDAO.AddUser(newUser);
                if (isSuccess)
                {
                    MessageBox.Show("Registration successful!");
                    this.Close();
                }
                else
                {
                    errorMessage.Text = "Registration failed. Username may already exist.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Login lo = new Login();
            lo.Show();
            this.Close();
        }
    }
}
using MusicStore.DataAccessLayer;
using MusicStore.Models;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public event Action LoginSuccessful;

        public bool IsLoginAdmin { get; private set; }
        public Login()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            User u = UserDAO.getUserbyUsernamePassword(txtUsername.Text,txtPassword.Password);
            if (u == null)
            {
                MessageBox.Show("This user does not exsist");
            }
            else 
            {


                if (u.RoleId == 1)
                {
                    AdminScreen ad = new AdminScreen();
                    
                    ad.Show(); // Hiện AdminScreen
                    IsLoginAdmin = true; // Đặt trạng thái đăng nhập thành công
                    this.Close();


                }
                else
                {
                    LoginSuccessful?.Invoke(); // Gọi sự kiện nếu không phải Admin
                    IsLoginAdmin = false; // Đặt trạng thái không thành công
                    this.Close(); // Đóng cửa sổ đăng nhập

                }




            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }



    }
}

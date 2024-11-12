using MusicStore.DataAccessLayer;
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
    /// Interaction logic for OrderConfirmation.xaml
    /// </summary>
    public partial class OrderConfirmation : Window
    {
        private readonly MusicStoreContext _context;
        private readonly int _orderId;

        public OrderConfirmation(int orderId)
        {
            InitializeComponent();
            _context = new MusicStoreContext();
            _orderId = orderId;
            LoadOrderDetails();
        }

        private void LoadOrderDetails()
        {
            // Tải thông tin đơn hàng từ cơ sở dữ liệu dựa trên OrderId
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == _orderId);

            if (order != null)
            {
                // Hiển thị tổng giá trị đơn hàng
                TotalTextBlock.Text = $"Total: ${order.Total}";

                // Cập nhật các trường nhập liệu (nếu có sẵn thông tin từ đơn hàng)
                FirstNameTextBox.Text = order.FirstName ?? string.Empty;
                LastNameTextBox.Text = order.LastName ?? string.Empty;
                AddressTextBox.Text = order.Address ?? string.Empty;
                CityTextBox.Text = order.City ?? string.Empty;
                StateTextBox.Text = order.State ?? string.Empty;
                CountryTextBox.Text = order.Country ?? string.Empty;
                PhoneTextBox.Text = order.Phone ?? string.Empty;
                EmailTextBox.Text = order.Email ?? string.Empty;

                if (string.IsNullOrEmpty(FirstNameTextBox.Text) || string.IsNullOrEmpty(LastNameTextBox.Text) ||
    string.IsNullOrEmpty(AddressTextBox.Text) || string.IsNullOrEmpty(CityTextBox.Text) ||
    string.IsNullOrEmpty(StateTextBox.Text) || string.IsNullOrEmpty(CountryTextBox.Text) ||
    string.IsNullOrEmpty(PhoneTextBox.Text) || string.IsNullOrEmpty(EmailTextBox.Text))
                {
                    MessageBox.Show("Some fields are empty.");
                }
            }
            else
            {
                MessageBox.Show("Order not found.");
                this.Close();
            }
        }

        private void SubmitOrderButton_Click(object sender, RoutedEventArgs e)
        {
            // Lấy đơn hàng từ cơ sở dữ liệu
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == _orderId);

            if (order != null)
            {
                // Cập nhật thông tin đơn hàng với dữ liệu người dùng nhập
                order.FirstName = FirstNameTextBox.Text;
                order.LastName = LastNameTextBox.Text;
                order.Address = AddressTextBox.Text;
                order.City = CityTextBox.Text;
                order.State = StateTextBox.Text;
                order.Country = CountryTextBox.Text;
                order.Phone = PhoneTextBox.Text;
                order.Email = EmailTextBox.Text;

                // Lưu thay đổi vào cơ sở dữ liệu
                _context.SaveChanges();

                MessageBox.Show("Order submitted successfully!");
                
                var user = UserDAO.GetLoggedInUser();
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close(); // Đóng cửa sổ OrderConfirmation
            }
            else
            {
                MessageBox.Show("Failed to update order. Please try again.");
            }
        }
    }
}


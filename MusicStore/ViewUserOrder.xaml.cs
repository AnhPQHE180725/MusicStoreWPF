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
    /// Interaction logic for ViewUserOrder.xaml
    /// </summary>
    public partial class ViewUserOrder : Window
    {
        private readonly MusicStoreContext _context;
        private readonly int _loggedInUserId;
        private readonly User user = UserDAO.GetLoggedInUser();
        public ViewUserOrder()
        {
            InitializeComponent();
            _context = new MusicStoreContext();
            _loggedInUserId = user.UserId;  // Save the logged-in user ID
            LoadUserOrders();
        }
        // Load only the orders of the logged-in user
        private void LoadUserOrders()
        {
            var orders = _context.Orders
                                 .Where(o => o.UserId == _loggedInUserId)  // Filter by logged-in user's ID
                                 .Select(o => new
                                 {
                                     o.OrderId,
                                     o.OrderDate,
                                     o.FirstName,
                                     o.LastName,
                                     o.Address,
                                     o.City,
                                     o.State,
                                     o.Country,
                                     o.Phone,
                                     o.Email,
                                     o.Total
                                 })
                                 .ToList();

            ListOrder.ItemsSource = orders;
        }

        // Load Order Details into the OrderDetails DataGrid based on selected order
        private void LoadOrderDetails(int orderId)
        {
            var orderDetails = _context.OrderDetails
                .Where(od => od.OrderId == orderId)
                .Select(od => new
                {
                    od.OrderDetailId,
                    od.OrderId,
                    od.AlbumId,
                    AlbumTitle = od.Album.Title, // Include album title in the details
                    od.Quantity,
                    od.UnitPrice
                })
                .ToList();

            ListOrderDetail.ItemsSource = orderDetails;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow ad = new MainWindow();
            ad.Show();
            this.Close();
        }

        private void ListOrderDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListOrder.SelectedItem is Order selectedOrder)  // Cast the selected item to dynamic
            {
                int orderId = selectedOrder.OrderId;
                LoadOrderDetails(orderId);  // Load the details of the selected order
            }
        }
    }
}


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
    /// Interaction logic for ViewOrder.xaml
    /// </summary>
    public partial class ViewOrder : Window
    {
        private readonly MusicStoreContext _context;

        public ViewOrder()
        {
            InitializeComponent();
            _context = new MusicStoreContext();
            LoadOrders();
        }
        private void LoadOrders()
        {
            var orders = _context.Orders.ToList();

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
                    AlbumTitle = od.Album.Title, 
                    od.Quantity,
                    od.UnitPrice
                })
                .ToList();

            ListOrderDetail.ItemsSource = orderDetails;
        }


        private void Back_Click(object sender, RoutedEventArgs e)
        {
        AdminScreen ad = new AdminScreen();
            ad.Show();
            this.Close();
        }

        private void ListOrderDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListOrder.SelectedItem is Order selectedOrder)  // Cast the selected item to Order
            {
                int orderId = selectedOrder.OrderId;
                LoadOrderDetails(orderId);  // Load the details of the selected order
            }

        }
    }
}

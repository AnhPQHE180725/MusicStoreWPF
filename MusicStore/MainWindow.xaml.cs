using Microsoft.EntityFrameworkCore;
using MusicStore.DataAccessLayer;
using MusicStore.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MusicStore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MusicStoreContext _context;
        private readonly User checkLogin = UserDAO.GetLoggedInUser();



        public MainWindow()
        {
            InitializeComponent();
            _context = new MusicStoreContext();
            UpdateLoginVisibility();

        }
        private void UpdateLoginVisibility()
        {
            if (checkLogin != null)
            {
                LoginButton.Visibility = Visibility.Hidden;
                Logout.Visibility = Visibility.Visible;
                Viewmyorder.Visibility = Visibility.Visible;
            }
            else
            {
                LoginButton.Visibility = Visibility.Visible;
                Logout.Visibility = Visibility.Hidden;
                Viewmyorder.Visibility = Visibility.Hidden;

            }
        }
        private void Main_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAlbums();
            loadGenreFind();
            loadArtistFind();

 
        }
        private void LoadAlbums()
        {
            var albums = _context.Albums.Include(a => a.Artist).Include(a => a.Genre).ToList();
            albumList.ItemsSource = albums;
        }
        private void loadArtistFind()
        {
            var artist = _context.Artists.ToList();
            ArtistListBox.ItemsSource = artist;
        }


        private void loadGenreFind()
        {
            var genre = _context.Genres.ToList();
            cboGenreFind.ItemsSource = genre;
            cboGenreFind.DisplayMemberPath = "Name";
            cboGenreFind.SelectedValue = "ArtistId";
        }

        private void AlbumListGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (albumList.SelectedItem is Album selectedAlbum)
            {
                LoadSongsForAlbum(selectedAlbum.AlbumId); 

            }
        }
        private void AddCartButton_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra xem album nào đang được chọn
            if (albumList.SelectedItem is Album selectedAlbum)
            {
                // Kiểm tra xem album đã tồn tại trong giỏ hàng chưa
                var existingItem = CartListBox.Items.Cast<OrderDetail>().FirstOrDefault(item => item.AlbumId == selectedAlbum.AlbumId);

                if (existingItem != null)
                {
                    // Nếu album đã tồn tại, tăng số lượng
                    existingItem.Quantity++;
                    existingItem.UnitPrice += selectedAlbum.Price;// Cập nhật giá
                    CartListBox.Items.Refresh();
                }
                else
                {
                    // Nếu album chưa tồn tại, thêm mới vào giỏ hàng
                    var orderDetail = new OrderDetail
                    {
                        AlbumId = selectedAlbum.AlbumId,
                        Quantity = 1,
                        UnitPrice = selectedAlbum.Price,
                        Album = selectedAlbum
                    };
                    CartListBox.Items.Add(orderDetail);
                }

                // Cập nhật tổng giá
                UpdateTotalPrice();
            }
        }
        private void DeleteFromCartButton_Click(object sender, RoutedEventArgs e)
        {
            if (CartListBox.SelectedItem is OrderDetail selectedOrderDetail)
            {
                CartListBox.Items.Remove(selectedOrderDetail);
                UpdateTotalPrice(); // Cập nhật tổng giá sau khi xóa
            }
        }

        private void UpdateTotalPrice()
        {
            decimal totalPrice = CartListBox.Items.Cast<OrderDetail>().Sum(item => item.UnitPrice);
            totalPriceTextBlock.Text = $"Total: ${totalPrice:F2}";
        }


        private void LoadSongsForAlbum(int albumId)
        {
            var songs = _context.Songs
                                .Where(s => s.AlbumId == albumId)
                                .ToList();

            SongListBox.ItemsSource = songs;
        }


        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {

            Login lo = new Login();
                lo.Show();
                this.Close();
            
        }



        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterAlbums();

        }



        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            albumList.SelectedValue = 0;
            cboGenreFind.SelectedValue = null; 
            ArtistListBox.SelectedValue = null; 
            LoadAlbums(); 
        }

        private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void ArtistListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterAlbums();

        }
        private void FilterAlbums()
        {
            var selectedGenre = cboGenreFind.SelectedValue as Genre;
            var selectedArtist = ArtistListBox.SelectedValue as Artist;

            var filteredAlbums = _context.Albums.AsQueryable();

            if (selectedGenre != null)
            {
                filteredAlbums = filteredAlbums.Where(a => a.Genre == selectedGenre);
            }

            if (selectedArtist != null)
            {
                filteredAlbums = filteredAlbums.Where(a => a.Artist == selectedArtist);
            }



            albumList.ItemsSource = filteredAlbums.ToList();
        }

        private void SongListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {

            UserDAO.LogoutUser();
            Login lo = new Login();
            lo.Show();
            this.Close();


        }

        private void CheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (CartListBox.Items.Count == 0)
            {
                MessageBox.Show("Your cart is empty. Please add items to the cart before proceeding.");
                return; 
            }
            var loggedInUser = UserDAO.GetLoggedInUser();
            if (loggedInUser == null)
            {
                MessageBox.Show("Please login to proceed.");
                LoginButton_Click(sender, e);  // Mở cửa sổ đăng nhập
                return;
            }
            int newOrderId = _context.Orders.Any() ? _context.Orders.Max(a => a.OrderId) + 1 : 1;

            var order = new Order
            {
                OrderId = newOrderId,
                UserId = loggedInUser.UserId,  // Lưu lại UserId của người dùng
                OrderDate = DateOnly.FromDateTime(DateTime.Now),
                Total = 0  // Khởi tạo tổng giá trị đơn hàng
            };
            decimal totalPrice = 0;

            foreach (var item in CartListBox.Items)
            {

                if (item is OrderDetail orderDetail)
                {
                    var orderItem = new OrderDetail
                    {
                        OrderId = order.OrderId,
                        AlbumId = orderDetail.AlbumId,
                        Quantity = orderDetail.Quantity,
                        UnitPrice = orderDetail.UnitPrice
                    };

                    // Thêm chi tiết đơn hàng vào cơ sở dữ liệu
                    _context.OrderDetails.Add(orderItem);

                    // Cộng tổng giá trị đơn hàng
                    totalPrice += orderDetail.UnitPrice * orderDetail.Quantity;
                }
            }

            // Cập nhật tổng giá trị của đơn hàng
            order.Total = totalPrice;

            // Thêm đơn hàng vào cơ sở dữ liệu
            _context.Orders.Add(order);

            // Lưu các thay đổi vào cơ sở dữ liệu
            _context.SaveChanges();

            // Chuyển sang màn hình xác nhận đơn hàng

            OrderConfirmation orderConfirmationWindow = new OrderConfirmation(order.OrderId);
            orderConfirmationWindow.Show();

            this.Close();  // Đóng cửa sổ hiện tại
        }

        private void Viewmyorder_Click(object sender, RoutedEventArgs e)
        {
            ViewUserOrder viewUserOrder = new ViewUserOrder();
            viewUserOrder.Show();
            this.Close();
        }
    }
}
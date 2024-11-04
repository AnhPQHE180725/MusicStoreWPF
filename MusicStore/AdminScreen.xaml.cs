using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for AdminScreen.xaml
    /// </summary>
    public partial class AdminScreen : Window
    {
        private readonly MusicStoreContext _context;
        public AdminScreen()
        {
            InitializeComponent();
            _context = new MusicStoreContext();
        }

        private void Main_Loaded(object sender, RoutedEventArgs e)
        {
            getlistAlbum();
            getlistGenre();
            getlistArtist();
        }
        private void getlistAlbum()
        {
            var albums = _context.Albums.Include(a => a.Artist).Include(a => a.Genre).ToList();
            listAlbum.ItemsSource = albums;
        }
        private void getlistGenre()
        {
            var genres = _context.Genres.ToList();
            cboGenre.ItemsSource = genres;
            cboGenre.DisplayMemberPath = "Name";
            cboGenre.SelectedValue = "GenreId";

        }
        private void getlistArtist()
        {
            var artist = _context.Artists.ToList();
            cboArtist.ItemsSource = artist;
            cboArtist.DisplayMemberPath = "Name";
            cboArtist.SelectedValue = "ArtistId";


        }


        private void cboGenre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cboArtist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void Refesh_btn(object sender, RoutedEventArgs e)
        {
            getlistAlbum();
            ClearFields();

        }

        private void listAlbum_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listAlbum.SelectedItem is Album selectedAlbum)
            {
                LoadAlbumDetail(selectedAlbum.AlbumId);
            }

        }
        private void LoadAlbumDetail(int albumId)
        {
            var album = _context.Albums
                .Include(a => a.Artist)
                .Include(a => a.Genre)
                .FirstOrDefault(a => a.AlbumId == albumId);

            if (album != null)
            {
                txtAlbumId.Text = album.AlbumId.ToString();
                txtPrice.Text = album.Price.ToString() ?? string.Empty;
                txtAlbumUrl.Text = album.AlbumUrl ?? string.Empty;
                txtTitle.Text = album.Title ?? string.Empty;
                cboArtist.SelectedValue = album.Artist;
                cboGenre.SelectedValue = album.Genre;
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {

            // Validate the input fields before adding
            if (string.IsNullOrWhiteSpace(txtTitle.Text) ||
                string.IsNullOrWhiteSpace(txtPrice.Text) ||
                cboArtist.SelectedValue == null ||
                cboGenre.SelectedValue == null)
            {
                MessageBox.Show("Please fill in all fields before adding an album.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // Get the maximum AlbumId from the database
            int newAlbumId = _context.Albums.Any() ? _context.Albums.Max(a => a.AlbumId) + 1 : 1;
            var newAlbum = new Album
            {
                AlbumId = newAlbumId,
                Title = txtTitle.Text,
                AlbumUrl = txtAlbumUrl.Text,
                Price = decimal.TryParse(txtPrice.Text, out decimal price) ? price : 0,
                Artist = (Artist)cboArtist.SelectedValue,
                Genre = (Genre)cboGenre.SelectedValue
            };
            // Add the new album to the context and save changes
            _context.Albums.Add(newAlbum);
            _context.SaveChanges();

            // Refresh the album list and clear input fields
            getlistAlbum();
            ClearFields();


        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            // Check if an album is selected
            if (listAlbum.SelectedItem is Album selectedAlbum)
            {
                // Confirm deletion with the user
                var result = MessageBox.Show($"Are you sure you want to delete the album '{selectedAlbum.Title}'?",
                                               "Confirm Deletion",
                                               MessageBoxButton.YesNo,
                                               MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    // Find the album in the database
                    var albumToDelete = _context.Albums.FirstOrDefault(a => a.AlbumId == selectedAlbum.AlbumId);
                    if (albumToDelete != null)
                    {
                        // Remove the album and save changes
                        _context.Albums.Remove(albumToDelete);
                        _context.SaveChanges();

                        // Refresh the album list
                        getlistAlbum();
                        ClearFields(); // Optionally clear fields after deletion
                    }
                    else
                    {
                        MessageBox.Show("Album not found in the database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select an album to delete.", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (listAlbum.SelectedItem is not null)
            {
                dynamic selectAl = listAlbum.SelectedItem;
                var AId = (int)selectAl.AlbumId;
                var albumToUpdate = _context.Albums.FirstOrDefault(a => a.AlbumId == AId);
                if (albumToUpdate != null)
                {
                    albumToUpdate.Title = txtTitle.Text;

                    if (decimal.TryParse(txtPrice.Text, out decimal price))
                    {
                        albumToUpdate.Price = price;

                    }
                    albumToUpdate.AlbumUrl= txtAlbumUrl.Text;
                    albumToUpdate.Artist = (Artist)cboArtist.SelectedValue;
                    albumToUpdate.Genre = (Genre)cboGenre.SelectedValue ;
                    _context.SaveChanges();

                    getlistAlbum();
                    ClearFields();
                }

            }
        }
        private void ClearFields()
        {
            txtAlbumId.Clear();
            txtPrice.Clear();
            txtAlbumUrl.Clear();
            txtTitle.Clear();
            cboArtist.SelectedIndex = -1;
            cboGenre.SelectedIndex = -1;
        }

        private void cboArtistFind_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cboGenreFind_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

    }
}

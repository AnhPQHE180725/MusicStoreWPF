using Microsoft.EntityFrameworkCore;
using MusicStore.DataAccessLayer;
using MusicStore.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for Viewsong.xaml
    /// </summary>
    public partial class Viewsong : Window
    {
        private readonly MusicStoreContext _context;
        public Viewsong()
        {
            InitializeComponent();
            _context = new MusicStoreContext();
            LoadAlbums();
            LoadSongs();
        }
        // Load all albums into ComboBox
        private void LoadAlbums()
        {
            var albums = _context.Albums.ToList();
            AlbumComboBox.ItemsSource = albums;
            AlbumComboBox.DisplayMemberPath = "Title";  // Show album titles
            AlbumComboBox.SelectedValuePath = "AlbumId"; // Bind AlbumId
        }

        // Load all songs into DataGrid
        private void LoadSongs()
        {
            var songs = _context.Songs.Include(s => s.Album).ToList();
            SongsDataGrid.ItemsSource = songs;
        }

        // Add a new song
        private void AddSong_Click(object sender, RoutedEventArgs e)
        {
            var title = TitleTextBox.Text;
            var durationString = DurationTextBox.Text;
            var fileUrl = FileUrlTextBox.Text;
            var albumId = (int?)AlbumComboBox.SelectedValue;

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(durationString))
            {
                MessageBox.Show("Please fill in all the fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!TimeOnly.TryParse(durationString, out var duration))
            {
                MessageBox.Show("Invalid duration format. Please use HH:mm:ss.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            int newSongId = _context.Songs.Any() ? _context.Songs.Max(a => a.SongId) + 1 : 1;

            var song = new Song
            {
                SongId = newSongId,
                Title = title,
                Duration = duration,
                FileUrl = fileUrl,
                AlbumId = albumId
            };

            _context.Songs.Add(song);
            _context.SaveChanges();
            LoadSongs();  // Refresh the DataGrid
            ClearFields(); // Clear the input fields
        }

        // Update the selected song
        private void UpdateSong_Click(object sender, RoutedEventArgs e)
        {
            if (SongsDataGrid.SelectedItem is Song selectedSong)
            {
                selectedSong.Title = TitleTextBox.Text;
                selectedSong.Duration = TimeOnly.Parse(DurationTextBox.Text);
                selectedSong.FileUrl = FileUrlTextBox.Text;
                selectedSong.AlbumId = (int?)AlbumComboBox.SelectedValue;

                _context.Songs.Update(selectedSong);
                _context.SaveChanges();
                LoadSongs();  // Refresh the DataGrid
                ClearFields(); // Clear the input fields
            }
            else
            {
                MessageBox.Show("Please select a song to update.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Clear input fields
        private void ClearFields()
        {
            TitleTextBox.Clear();
            DurationTextBox.Clear();
            FileUrlTextBox.Clear();
            AlbumComboBox.SelectedIndex = -1;
        }

        // Populate fields when a song is selected from DataGrid
        private void SongsDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (SongsDataGrid.SelectedItem is Song selectedSong)
            {
                TitleTextBox.Text = selectedSong.Title;
                DurationTextBox.Text = selectedSong.Duration.ToString("HH:mm:ss");
                FileUrlTextBox.Text = selectedSong.FileUrl;
                AlbumComboBox.SelectedValue = selectedSong.AlbumId;
            }
        }
        // Refresh the DataGrid and the form fields
        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadSongs();  // Reload songs into DataGrid
            ClearFields(); // Clear the input fields
        }

        // Go back to the previous screen (or close the window)
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();  // Close the current window or navigate back to the previous screen
        }
    }
}
public class TimeOnlyToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is TimeOnly time)
        {
            // Convert TimeOnly to the desired format (hh:mm:ss)
            return time.ToString("hh\\:mm\\:ss");
        }
        return string.Empty; // Return empty if not a valid TimeOnly
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null; // Not needed for this scenario
    }
}


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
            AlbumComboBox.DisplayMemberPath = "Title";  
            AlbumComboBox.SelectedValuePath = "AlbumId"; 
        }

        private void LoadSongs()
        {
            var songs = _context.Songs.Include(s => s.Album).ToList();
            SongsDataGrid.ItemsSource = songs;
        }

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
            LoadSongs(); 
            ClearFields();
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
                LoadSongs();  
                ClearFields();
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
     
        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadSongs();  
            ClearFields(); 
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            AdminScreen ad = new AdminScreen();
            ad.Show();
            this.Close();  
        }
    }
}



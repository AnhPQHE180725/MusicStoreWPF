using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class AlbumDAO
    {
        public static List<Albums> GetAlbums()
        {
            var listAlbums = new List<Albums>();
            try
            {
                using (var db = new MusicStoreContext())
                {
                    listAlbums = db.Albums.ToList();
                }
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., to console or a logging framework)
                Console.WriteLine($"Error fetching albums: {ex.Message}");
                // Optionally, rethrow or handle the error as needed
            }
            return listAlbums;
        }
    }
}

using System;
using System.Collections.Generic;

namespace MusicStore.Models;

public partial class Album
{
    public int AlbumId { get; set; }

    public int? GenreId { get; set; }

    public int? ArtistId { get; set; }

    public string Title { get; set; } = null!;

    public decimal Price { get; set; }

    public string? AlbumUrl { get; set; }

    public virtual Artist? Artist { get; set; }

    public virtual Genre? Genre { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Song> Songs { get; set; } = new List<Song>();
}

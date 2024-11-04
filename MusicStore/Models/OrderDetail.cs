using System;
using System.Collections.Generic;

namespace MusicStore.Models;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int? OrderId { get; set; }

    public int? AlbumId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public virtual Album? Album { get; set; }

    public virtual Order? Order { get; set; }

    public override string ToString()
    {
        return $"{Album?.Title} - Quantity: {Quantity}, Price: {UnitPrice:C}";
    }
}

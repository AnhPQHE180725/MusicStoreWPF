using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Albums
    {
        public int AlbumId {  get; set; }
        public virtual Genres Genres { get; set; }
        public virtual Artists Artists { get; set; }
        public string Title {  get; set; }
        public decimal Price {  get; set; }
        public string AlbumUrl { get; set; }

        public ICollection<Songs> Songs { get; set; } = new List<Songs>();
        public ICollection<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();


    }
}

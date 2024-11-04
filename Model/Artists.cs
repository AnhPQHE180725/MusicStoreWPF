using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Artists
    {
        public int ArtistId {  get; set; }
        public string Name { get; set; }
        public ICollection<Albums> Albums { get; set; } = new List<Albums>();
    }
}

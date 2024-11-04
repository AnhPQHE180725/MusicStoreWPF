using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Songs
    {
        public int SongId { get; set; }
        public virtual Albums Albums { get; set; }

        public string Title {  get; set; }
        public TimeSpan Duration { get; set; }
        public string FileUrl {  get; set; }
    }
}

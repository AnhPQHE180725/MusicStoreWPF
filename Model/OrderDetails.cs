using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class OrderDetails
    {
        public int OrderDetailId {  get; set; }
        public virtual Orders Orders { get; set; }
        public virtual Albums Albums { get; set; }
        public int Quantity {  get; set; }
        public decimal UnitPrice {  get; set; }

    }
}

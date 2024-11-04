using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Orders
    {
        public int OrderId {  get; set; }
        public DateTime OrderDate { get; set; }
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State {  get; set; }
        public string Country {  get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public decimal  Total {  get; set; }

        public virtual User User { get; set; } 
        public ICollection<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();
    }
}

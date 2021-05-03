using System;
using System.Collections.Generic;

#nullable disable

namespace API.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderLines = new HashSet<OrderLine>();
        }

        public int OrderNo { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public int ManufacturerId { get; set; }
        public string ManufacturerName { get; set; }
        public string OrderedByName { get; set; }
        public bool Fulfilled { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
        public virtual CustomerInvoice CustomerInvoice { get; set; }
        public virtual ICollection<OrderLine> OrderLines { get; set; }
    }
}

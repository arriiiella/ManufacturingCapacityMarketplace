using System;
using System.Collections.Generic;

#nullable disable

namespace API.Models
{
    public partial class FeeTransaction
    {
        public Guid Id { get; set; }
        public Guid InvoiceId { get; set; }
        public decimal NetAmount { get; set; }
        public int ManufacturerId { get; set; }
        public decimal FeePercentage { get; set; }
        public decimal FeeAmount { get; set; }
        public decimal VatAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public byte Paid { get; set; }
        public DateTime Date { get; set; }

        public virtual CustomerInvoice Invoice { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
    }
}

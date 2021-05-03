using System;
using System.Collections.Generic;

#nullable disable

namespace API.Models
{
    public partial class CustomerInvoice
    {
        public Guid InvoiceNo { get; set; }
        public int OrderNo { get; set; }
        public int CustomerId { get; set; }
        public string VatRegistrationNo { get; set; }
        public DateTime Date { get; set; }
        public int ManufacturerId { get; set; }
        public decimal NetAmount { get; set; }
        public decimal VatAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public byte Paid { get; set; }
        public string PaymentRef { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
        public virtual Order OrderNoNavigation { get; set; }
        public virtual FeeTransaction FeeTransaction { get; set; }
    }
}

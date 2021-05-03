using System;
using System.Collections.Generic;

#nullable disable

namespace API.Models
{
    public partial class Manufacturer
    {
        public Manufacturer()
        {
            CustomerInvoices = new HashSet<CustomerInvoice>();
            ManufacturingLocations = new HashSet<ManufacturingLocation>();
            Notifications = new HashSet<Notification>();
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int IndustryId { get; set; }
        public int AddressId { get; set; }
        public string VatRegistrationNo { get; set; }
        public int? FeeRateId { get; set; }
        public int BillingAddressId { get; set; }

        public virtual Address Address { get; set; }
        public virtual Address BillingAddress { get; set; }
        public virtual FeeRate FeeRate { get; set; }
        public virtual Industry Industry { get; set; }
        public virtual FeeTransaction FeeTransaction { get; set; }
        public virtual ICollection<CustomerInvoice> CustomerInvoices { get; set; }
        public virtual ICollection<ManufacturingLocation> ManufacturingLocations { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}

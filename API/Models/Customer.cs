using System;
using System.Collections.Generic;

#nullable disable

namespace API.Models
{
    public partial class Customer
    {
        public Customer()
        {
            CustomerInvoices = new HashSet<CustomerInvoice>();
            Notifications = new HashSet<Notification>();
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? IndustryId { get; set; }
        public int AddressId { get; set; }
        public string VatRegistrationNo { get; set; }
        public int BillingAddressId { get; set; }
        public bool? IsPurchaseCapacity { get; set; }
        public bool? IsSellCapacity { get; set; }

        public virtual Address AddressNavigation { get; set; }
        public virtual Industry Industry { get; set; }
        public virtual Address Address { get; set; }
        public virtual ICollection<CustomerInvoice> CustomerInvoices { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}

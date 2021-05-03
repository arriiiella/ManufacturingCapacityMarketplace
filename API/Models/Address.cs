using System;
using System.Collections.Generic;

#nullable disable

namespace API.Models
{
    public partial class Address
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Postcode { get; set; }
        public string CountryCode { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public Guid UserId { get; set; }

        public virtual CountryCode CountryCodeNavigation { get; set; }
        public virtual Customer IdNavigation { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Manufacturer ManufacturerAddress { get; set; }
        public virtual Manufacturer ManufacturerBillingAddress { get; set; }
        public virtual ManufacturingLocation ManufacturingLocation { get; set; }
    }
}

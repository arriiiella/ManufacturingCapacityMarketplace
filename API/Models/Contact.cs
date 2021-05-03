using System;
using System.Collections.Generic;

#nullable disable

namespace API.Models
{
    public partial class Contact
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Salutation { get; set; }
        public int Type { get; set; }
        public int AddressId { get; set; }
        public int CustomerId { get; set; }
        public int? ManufacturerId { get; set; }
        public string Email { get; set; }
        public Guid? Userid { get; set; }
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace API.Models
{
    public partial class User
    {
        public Guid Userid { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? ManufacturerId { get; set; }
        public int? CustomerId { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Salutation { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
    }
}

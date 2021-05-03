using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class CustomerDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? IndustryId { get; set; }
        public int AddressId { get; set; }
        public string VatRegistrationNo { get; set; }
        public int BillingAddressId { get; set; }
        public bool? IsPurchaseCapacity { get; set; }
        public bool? IsSellCapacity { get; set; }
    }
}

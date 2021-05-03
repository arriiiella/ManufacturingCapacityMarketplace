using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class ManufacturerLocationDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ManufacturerId { get; set; }
        public int AddressId { get; set; }

        public virtual AddressDTO Address { get; set; }
        public virtual ManufacturerDTO Manufacturer { get; set; }
    }
}

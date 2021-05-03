using System;
using System.Collections.Generic;

#nullable disable

namespace API.Models
{
    public partial class ManufacturingLocation
    {
        public ManufacturingLocation()
        {
            Machines = new HashSet<Machine>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int ManufacturerId { get; set; }
        public int AddressId { get; set; }

        public virtual Address Address { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
        public virtual OrderLine OrderLine { get; set; }
        public virtual ICollection<Machine> Machines { get; set; }
    }
}

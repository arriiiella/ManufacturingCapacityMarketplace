using System;
using System.Collections.Generic;

#nullable disable

namespace API.Models
{
    public partial class Industry
    {
        public Industry()
        {
            Manufacturers = new HashSet<Manufacturer>();
            ManufacturingProcesses = new HashSet<ManufacturingProcess>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<Manufacturer> Manufacturers { get; set; }
        public virtual ICollection<ManufacturingProcess> ManufacturingProcesses { get; set; }
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace API.Models
{
    public partial class Machine
    {
        public Machine()
        {
            MachineCapacityEntries = new HashSet<MachineCapacityEntry>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int LocationId { get; set; }
        public string ModelNo { get; set; }
        public decimal Capacity { get; set; }
        public decimal SetupTime { get; set; }
        public decimal CostPerUnit { get; set; }
        public int ManufacturingProcessId { get; set; }

        public virtual ManufacturingLocation Location { get; set; }
        public virtual ManufacturingProcess ManufacturingProcess { get; set; }
        public virtual OrderLine OrderLine { get; set; }
        public virtual ICollection<MachineCapacityEntry> MachineCapacityEntries { get; set; }
    }
}

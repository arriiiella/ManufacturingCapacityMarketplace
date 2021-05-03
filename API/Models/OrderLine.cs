using System;
using System.Collections.Generic;

#nullable disable

namespace API.Models
{
    public partial class OrderLine
    {
        public int OrderNo { get; set; }
        public int LineNo { get; set; }
        public int LocationId { get; set; }
        public int MachineId { get; set; }
        public decimal Capacity { get; set; }
        public decimal CostPerUnit { get; set; }
        public decimal LineAmount { get; set; }
        public int CapacityEntryNo { get; set; }

        public virtual MachineCapacityEntry CapacityEntryNoNavigation { get; set; }
        public virtual ManufacturingLocation Location { get; set; }
        public virtual Machine Machine { get; set; }
        public virtual Order OrderNoNavigation { get; set; }
    }
}

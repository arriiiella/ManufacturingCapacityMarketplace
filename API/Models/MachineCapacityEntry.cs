using System;
using System.Collections.Generic;

#nullable disable

namespace API.Models
{
    public partial class MachineCapacityEntry
    {
        public int EntryNo { get; set; }
        public int MachineId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public decimal Capacity { get; set; }

        public virtual Machine Machine { get; set; }
        public virtual OrderLine OrderLine { get; set; }
    }
}

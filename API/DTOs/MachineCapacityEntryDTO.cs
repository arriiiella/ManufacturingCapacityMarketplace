using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class MachineCapacityEntryDTO
    {
        public int EntryNo { get; set; }
        public int MachineId { get; set; }
        public string Date { get; set; }
        public string StartTime { get; set; }
        public decimal Capacity { get; set; }
    }
}

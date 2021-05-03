using Microsoft.EntityFrameworkCore;
using System;

namespace API.Models
{
    [Keyless]
    public class SpareCapacity
    {
        public int IndustryId { get; set; }
        public string IndustryName { get; set; }
        public int ProcessId { get; set; }
        public string ProcessName { get; set; }
        public int ManufacturerId { get; set; }
        public string ManufacturerName { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string City { get; set; }
        public int MachineId { get; set; }
        public string MachineName { get; set; }
        public string ModelNo { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public decimal Capacity { get; set; }
        public decimal CostPerUnit { get; set; }
        public decimal CapacityCost { get; set; }
        public int CapacityEntryNo { get; set; }
    }
}

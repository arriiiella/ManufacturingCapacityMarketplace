using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class OrderWithDetailsDTO
    {
        public int OrderNumber { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public int ManufacturerId { get; set; }
        public string ManufacturerName { get; set; }
        public string OrderedByName { get; set; }
        public bool Fulfilled { get; set; }
        public int LineNumber { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public int MachineId { get; set; }
        public string MachineName { get; set; }
        public string ModelNo { get; set; }
        public TimeSpan StartTime { get; set; }
        public decimal Capacity { get; set; }
        public decimal CostPerUnit { get; set; }
        public decimal LineAmount { get; set; }
        public int CapacityEntryNo { get; set; }
    }
}

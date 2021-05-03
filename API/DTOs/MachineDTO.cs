using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class MachineDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LocationId { get; set; }
        public string ModelNo { get; set; }
        public decimal Capacity { get; set; }
        public decimal SetupTime { get; set; }
        public decimal CostPerUnit { get; set; }
        public int ManufacturingProcessId { get; set; }
        public virtual ManufacturingProcessDTO ManufacturingProcess { get; set; }
    }
}

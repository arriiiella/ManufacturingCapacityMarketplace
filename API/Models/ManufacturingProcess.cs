using System;
using System.Collections.Generic;

#nullable disable

namespace API.Models
{
    public partial class ManufacturingProcess
    {
        public ManufacturingProcess()
        {
            Machines = new HashSet<Machine>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int IndustryId { get; set; }

        public virtual Industry Industry { get; set; }
        public virtual ICollection<Machine> Machines { get; set; }
    }
}

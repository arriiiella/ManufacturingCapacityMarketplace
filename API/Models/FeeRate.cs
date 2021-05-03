using System;
using System.Collections.Generic;

#nullable disable

namespace API.Models
{
    public partial class FeeRate
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Percentage { get; set; }

        public virtual Manufacturer Manufacturer { get; set; }
    }
}

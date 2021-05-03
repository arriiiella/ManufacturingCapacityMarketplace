using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class ManufacturingProcessDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int IndustryId { get; set; }
    }
}

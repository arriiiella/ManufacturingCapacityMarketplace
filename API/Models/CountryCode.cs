using System;
using System.Collections.Generic;

#nullable disable

namespace API.Models
{
    public partial class CountryCode
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual Address Address { get; set; }
    }
}

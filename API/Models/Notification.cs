using System;
using System.Collections.Generic;

#nullable disable

namespace API.Models
{
    public partial class Notification
    {
        public int Id { get; set; }
        public int FromCustomerId { get; set; }
        public int ToManufacturerId { get; set; }
        public string MessageContent { get; set; }
        public int RelatesToNotificationId { get; set; }
        public int Sequence { get; set; }
        public DateTime DateSent { get; set; }

        public virtual Customer FromCustomer { get; set; }
        public virtual Manufacturer ToManufacturer { get; set; }
    }
}

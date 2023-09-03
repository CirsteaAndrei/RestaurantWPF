using Restaurant.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models.Entities
{
    public class Order : BaseEntity
    {
        public OrderStatusEnum OrderStatusEnum { get; set; }
        public float? TotalPrice { get; set; }
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;

    }
}

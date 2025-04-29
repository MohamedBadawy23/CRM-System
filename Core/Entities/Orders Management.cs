using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Orders_Management : BaseEntity
    {
        public int? OrderNumber { get; set; }
        public decimal? Price { get; set; }

        public String? Product { get; set; }
     
        public DateTime? OrderDate { get; set; } = DateTime.Now;
        public string? Status { get; set; } = "Pending";

        public string? UserId { get; set; }
        public User? User { get; set; }
        public int? ClientId { get; set; }
        public Clients_Management? Client { get; set; }

    }
}

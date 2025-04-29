using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Clients_Management : BaseEntity
    {
        public string? Name { get; set; } 
        public int ? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public DateTime? Date { get; set; } = DateTime.Now;

        //public List<Orders_Management>? Orders { get; set; } = new List<Orders_Management>();
        public string? UserId { get; set; }
        public User? User { get; set; }
      //  public int? OrderId { get; set; }
      //  public List<Orders_Management>? Orders { get; set; } = new List<Orders_Management>();

    }
}

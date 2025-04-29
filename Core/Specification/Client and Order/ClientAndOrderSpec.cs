using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Specification.Client_and_Order
{
    public class ClientAndOrderSpec:BaseSpecification<Orders_Management>
    {
       // public string? Sort { get; set; } 
        public ClientAndOrderSpec(String?Sort ) 
        
        {
            Includes.Add(x => x.User);

            switch (Sort)
            {
                case
                    "Name":
                    OrderByAsc(p => p.OrderNumber);
                    break;
                case "Date":
                    OrderByAsc(p => p.OrderDate);
                    break;
                case "Price":
                    OrderByAsc(p => p.Price);
                    break;
                case "Status":
                    OrderByAsc(p => p.Status);
                    break;
                default:
                    OrderByDesc(p => p.OrderDate);
                    break;




            }   }
    }
}

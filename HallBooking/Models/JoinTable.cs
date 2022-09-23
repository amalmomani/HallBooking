using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HallBooking.Models
{
    public class JoinTable
    {
        public Useraccount user { get; set; }
        public Book booking { get; set; }
        public Hall halls { get; set; }
        public Hallcategory category { get; set; }
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace HallBooking.Models
{
    public partial class Activeday
    {
        public Activeday()
        {
            Hallactives = new HashSet<Hallactive>();
        }

        public decimal Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Hallactive> Hallactives { get; set; }
    }
}

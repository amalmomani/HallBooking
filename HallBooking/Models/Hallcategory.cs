using System;
using System.Collections.Generic;

#nullable disable

namespace HallBooking.Models
{
    public partial class Hallcategory
    {
        public Hallcategory()
        {
            Halls = new HashSet<Hall>();
        }

        public decimal Categoryid { get; set; }
        public string Name { get; set; }
        public string Imagepath { get; set; }

        public virtual ICollection<Hall> Halls { get; set; }
    }
}

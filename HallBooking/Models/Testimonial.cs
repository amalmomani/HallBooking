using System;
using System.Collections.Generic;

#nullable disable

namespace HallBooking.Models
{
    public partial class Testimonial
    {
        public decimal Testmoninalid { get; set; }
        public string Message { get; set; }
        public string Testimage { get; set; }
        public bool? Status { get; set; }
        public decimal? Userid { get; set; }
        public string Name { get; set; }

        public virtual Useraccount User { get; set; }
    }
}

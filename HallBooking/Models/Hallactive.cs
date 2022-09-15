using System;
using System.Collections.Generic;

#nullable disable

namespace HallBooking.Models
{
    public partial class Hallactive
    {
        public decimal Id { get; set; }
        public decimal? Hallid { get; set; }
        public decimal? Activedayid { get; set; }
        public bool? Isbook { get; set; }

        public virtual Activeday Activeday { get; set; }
        public virtual Hall Hall { get; set; }
    }
}

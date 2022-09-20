using System;
using System.Collections.Generic;

#nullable disable

namespace HallBooking.Models
{
    public partial class Book
    {
        public decimal Id { get; set; }
        public decimal? Userid { get; set; }
        public decimal? Hallid { get; set; }
        public DateTime? Startdate { get; set; }
        public DateTime? Enddate { get; set; }
        public string Status { get; set; }

        public virtual Hall Hall { get; set; }
        public virtual Useraccount User { get; set; }
    }
}

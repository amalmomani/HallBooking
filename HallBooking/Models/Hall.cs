using System;
using System.Collections.Generic;

#nullable disable

namespace HallBooking.Models
{
    public partial class Hall
    {
        public Hall()
        {
            Books = new HashSet<Book>();
        }

        public decimal Hallid { get; set; }
        public string Hallname { get; set; }
        public string Hallddress { get; set; }
        public decimal? Hallsize { get; set; }
        public decimal? Price { get; set; }
        public bool? Isbooked { get; set; }
        public decimal? Categoryid { get; set; }

        public virtual Hallcategory Category { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}

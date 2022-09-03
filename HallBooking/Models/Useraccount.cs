using System;
using System.Collections.Generic;

#nullable disable

namespace HallBooking.Models
{
    public partial class Useraccount
    {
        public Useraccount()
        {
            Books = new HashSet<Book>();
            Payments = new HashSet<Payment>();
            Testimonials = new HashSet<Testimonial>();
        }

        public decimal Userid { get; set; }
        public string Fullname { get; set; }
        public decimal? Phonenumber { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public decimal? Roleid { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<Book> Books { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Testimonial> Testimonials { get; set; }
    }
}

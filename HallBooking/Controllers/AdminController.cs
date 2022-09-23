using HallBooking.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HallBooking.Controllers
{
    public class AdminController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment webhostEnvironment;
        public AdminController(ModelContext context, IWebHostEnvironment _webHostEnvironment)
        {
            _context = context;
            //assign initial value variable
            webhostEnvironment = _webHostEnvironment;
        }
        public IActionResult Index()
        {

            ViewBag.Fullname = HttpContext.Session.GetString("Fullname");
            ViewBag.Userid = HttpContext.Session.GetInt32("Userid");
            ViewBag.Email = HttpContext.Session.GetString("Email");


            ViewBag.numberofcustomer = _context.Useraccounts.Count();
            ViewBag.numberofHalls = _context.Halls.Count();
            ViewBag.numberofBook = _context.Books.Count();
            ViewBag.number_of_Hallcategories = _context.Hallcategories.Count();


            ViewBag.numberofcategories = _context.Hallcategories.Count();

            var user = _context.Useraccounts.ToList().Take(5);
            var category = _context.Hallcategories.ToList().Take(3);
            var hall = _context.Halls.ToList();

            var modle = Tuple.Create<IEnumerable<Useraccount>, IEnumerable<Hallcategory>, IEnumerable<Hall>>(user, category,hall);







            return View(modle);
        }



        public IActionResult Reports()
        {
            var user = _context.Useraccounts.ToList();
            var book = _context.Books.ToList();
            var hall = _context.Halls.ToList();
            var Hallcategory = _context.Hallcategories.ToList();

            var result = from u in user
                         join b in book on u.Userid equals b.Userid
                         join h in hall on b.Userid equals h.Hallid
                         join hc in Hallcategory on h.Hallid equals hc.Categoryid
                         select new JoinTable { user = u, booking = b, halls = h, category = hc };


            return View(result);
        }

        public IActionResult Search()
        {
            var modelContext = _context.Books.Where(x => x.Status == "Accept" || x.Status == "Paied").Include(p => p.Hall).Include(p => p.User);

            return View(modelContext.ToList());
        }
        [HttpPost]
        public IActionResult Search(DateTime? startDate , DateTime? endDate)
        {
            var modelContext = _context.Books.Where(x=>x.Status == "Accept" || x.Status == "Paied").Include(p => p.Hall).Include(p => p.User);

            if (startDate == null && endDate == null)
            {
                var model = _context.Books.Where(x => x.Status == "Accept" || x.Status == "Paied").Include(p => p.Hall).Include(p => p.User);
                return View(model.ToList());
            }
            else if (startDate != null && endDate == null)
            {
                var result1 = modelContext.Where(x => x.Startdate.Value.Date >= startDate && (x.Status=="Accept"||x.Status=="Paied")).Include(p => p.Hall).Include(p => p.User);
              
                return View(result1);
            }
            else if (startDate == null && endDate != null)
            {
                var result = modelContext.Where(x => x.Enddate.Value.Date <= endDate &&(x.Status == "Accept" || x.Status == "Paied")).Include(p => p.Hall).Include(p => p.User);
              
                return View(result.ToList());
            }
            else
            {
                var result = modelContext.Where(x => x.Startdate.Value.Date <= endDate && x.Enddate.Value.Date >= startDate &&(x.Status == "Accept" || x.Status == "Paied")).Include(p => p.Hall).Include(p => p.User);
               
                return View(result.ToList());
            }

        }

    }
}

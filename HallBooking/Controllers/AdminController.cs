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



        public async Task<IActionResult> Search()
        {
            var modelContext = _context.Books.Include(b => b.Hall).Include(b => b.User);
            return View(await modelContext.ToListAsync());
        }
        [HttpPost]
        public IActionResult Search(DateTime? startDate , DateTime? EndDate,bool? Status)
        {

            return View();
        }

    }
}

using HallBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HallBooking.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
       
        private readonly ModelContext _context;
       


        public HomeController(ILogger<HomeController> logger, ModelContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.numberofHalls_isBooking = _context.Books.Count(x => x.Status == "Accept" );
            ViewBag.number_of_Hallcategories = _context.Hallcategories.Count();
            ViewBag.numberofcustomer = _context.Testimonials.Count();
            ViewBag.numberofHalls = _context.Halls.Count();
            var category = _context.Hallcategories.ToList().Take(3);
            var Homepage = _context.Mainpages.ToList();
            var about = _context.Aboutus.ToList();
            var halls = _context.Halls.ToList().Take(4);
            var home = Tuple.Create<IEnumerable<Hallcategory>, IEnumerable<Mainpage>,IEnumerable<Aboutu>,IEnumerable<Hall>>(category, Homepage, about, halls);
            return View(home);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

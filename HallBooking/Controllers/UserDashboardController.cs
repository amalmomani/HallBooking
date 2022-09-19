using HallBooking.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HallBooking.Controllers
{
    public class UserDashboardController : Controller
    {

        private readonly ModelContext _context;
        public UserDashboardController(ModelContext context)
        {
            _context = context;
            //assign initial value variable
        }
        public IActionResult Index(int id)
        {
            var holls = _context.Halls.Where(x => x.Categoryid == id);
            return View(holls);
        }
        public async Task<IActionResult> Category()
        {
            return View(await _context.Hallcategories.ToListAsync());
        }
        public async Task<IActionResult> Halls()
        {
            var modelContext = _context.Halls.Include(h => h.Category);
            return View(await modelContext.ToListAsync());
        }
        public IActionResult Add(int id)
        {
            ViewBag.Userid = HttpContext.Session.GetInt32("Userid");
            var hall = _context.Halls.Where(x => x.Hallid == id);

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int id, DateTime startDate, DateTime endDate)
        {
            ViewBag.Userid = HttpContext.Session.GetInt32("Userid");

            var hall = _context.Halls.Where(x => x.Hallid == id).FirstOrDefault();
            Book book = new Book();
            book.Userid = ViewBag.Userid;
            book.Startdate = startDate;
            book.Enddate = endDate;
            book.Hallid = id;
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return View();
        }
    }
}

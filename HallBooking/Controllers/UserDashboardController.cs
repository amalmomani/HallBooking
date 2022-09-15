using HallBooking.Models;
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
    }
}

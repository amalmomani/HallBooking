using HallBooking.Models;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index(int Categoryid)
        {
            var holls = _context.Halls.Where(x => x.Hallid == Categoryid);
            return View(holls);
        }
    }
}

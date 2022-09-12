using HallBooking.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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
            ViewBag.numberofcustomer = _context.Useraccounts.Count();
            ViewBag.numberofHalls = _context.Halls.Count();
            ViewBag.numberofBook = _context.Banks.Count();
            
            ViewBag.numberofcategories = _context.Hallcategories.Count();










            return View();
        }
    }
}

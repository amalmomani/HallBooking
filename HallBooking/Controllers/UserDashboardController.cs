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
            book.Status = false;
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return View();
        }
        public IActionResult Payment()
        {
            ViewBag.Userid = HttpContext.Session.GetInt32("Userid");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Payment(decimal Cardnumber, decimal cvv)
        {
            ViewBag.Userid = HttpContext.Session.GetInt32("Userid");
            ViewBag.flag = 1;
            ViewBag.emptyorder = 1;
            ViewBag.amount = 1;
            int u = ViewBag.Userid;
            List<Book> book = _context.Books.Where(x => x.Userid == u && x.Status==false).ToList();

            if (book != null)
            {
                decimal? amountt = 0;
                var bank = _context.Banks.Where(x => x.Cardnumber == Cardnumber && x.Cvv == cvv).FirstOrDefault();

                if (bank != null)
                {                   
                    Payment p = new Payment();
                    p.Cardnumber = Cardnumber;
                    foreach (Book b in book)
                    {                        
                        var hall = _context.Halls.Where(x => x.Hallid == b.Hallid).FirstOrDefault();
                        amountt = amountt + hall.Price;  
                    }
                    if (bank.Amount >= amountt)
                    {
                        p.Amount = amountt;
                        p.Paydate = DateTime.Now;
                        p.Userid = ViewBag.Userid;
                        _context.Payments.Add(p);
                        await _context.SaveChangesAsync();
                        foreach (Book b in book)
                        {
                            b.Status = true;
                            _context.Update(b);
                            await _context.SaveChangesAsync();
                        }
                        bank.Amount = bank.Amount - amountt;
                        _context.Update(bank);
                        await _context.SaveChangesAsync();
                    }
                    else
                        ViewBag.amount = 0;

                }
                else
                {
                    ViewBag.flag = 0;
                }
                await _context.SaveChangesAsync();

            }
            else if (book == null)
            {
                ViewBag.emptyorder = 0;
            }
            return View();
        }
    }
}

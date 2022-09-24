using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HallBooking.Models;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Mail;

namespace HallBooking.Controllers
{
    public class BooksController : Controller
    {
        private readonly ModelContext _context;

        public BooksController(ModelContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            ViewBag.Userid = HttpContext.Session.GetInt32("Userid");
            var modelContext = _context.Books.Include(b => b.Hall).Include(b => b.User);
            return View(await modelContext.ToListAsync());
        }
        public async Task<IActionResult> AcceptBook()
        {
            ViewBag.Userid = HttpContext.Session.GetInt32("Userid");
            var modelContext = _context.Books.Include(b => b.Hall).Include(b => b.User);
            return View(await modelContext.ToListAsync());
        }
        public async Task<IActionResult> Payment()
        {
            ViewBag.Userid = HttpContext.Session.GetInt32("Userid");
            var modelContext = _context.Books.Include(b => b.Hall).Include(b => b.User);
            return View(await modelContext.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Hall)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }
      
        public IActionResult Create()
        {
            ViewData["Hallid"] = new SelectList(_context.Halls, "Hallid", "Hallid");
            ViewData["Userid"] = new SelectList(_context.Useraccounts, "Userid", "Userid");
            return View();
        }

        // POST: Books1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Userid,Hallid,Startdate,Enddate,Status")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Hallid"] = new SelectList(_context.Halls, "Hallid", "Hallid", book.Hallid);
            ViewData["Userid"] = new SelectList(_context.Useraccounts, "Userid", "Userid", book.Userid);
            return View(book);
        }



        // GET: Books/Create
        //public IActionResult Create(decimal Hallid)
        //{
        //    ViewBag.Userid = HttpContext.Session.GetInt32("Userid");
        //    // ViewData["Hallid"] = new SelectList(_context.Halls, "Hallid", "Hallid");
        //    //ViewData["Userid"] = new SelectList(_context.Useraccounts, "Userid", "Userid");
        //    //var hall = _context.Halls.Where(x => x.Hallid == id).FirstOrDefault();

        //    return View();
        //}

        //// POST: Books/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Userid,Hallid,Startdate,Enddate,Status")] Book book)
        //{
        //    ViewBag.Userid = HttpContext.Session.GetInt32("Userid");
        //    book.Userid = ViewBag.Userid;

        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(book);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    //ViewData["Hallid"] = new SelectList(_context.Halls, "Hallid", "Hallid", book.Hallid);
        //    return View(book);
        //}

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["Hallid"] = new SelectList(_context.Halls, "Hallid", "Hallid", book.Hallid);
            ViewData["Userid"] = new SelectList(_context.Useraccounts, "Userid", "Userid", book.Userid);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Userid,Hallid,Startdate,Enddate,Status")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                   
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                var auth = _context.Useraccounts.Where(data => data.Userid == book.Userid).FirstOrDefault();
                SendEmail(auth.Email);
                return RedirectToAction(nameof(AcceptBook));
            }
            ViewData["Hallid"] = new SelectList(_context.Halls, "Hallid", "Hallid", book.Hallid);
            ViewData["Userid"] = new SelectList(_context.Useraccounts, "Userid", "Userid", book.Userid);
            return RedirectToAction(nameof(AcceptBook));
        }

        public IActionResult SendEmail(string to)
        {
            ViewBag.Fullname = HttpContext.Session.GetString("Fullname");
            ViewBag.Userid = HttpContext.Session.GetInt32("Userid");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            string e = ViewBag.Email;
            using System.Net.Mail.SmtpClient mySmtpClient = new System.Net.Mail.SmtpClient("smtp.outlook.com", 587);
            mySmtpClient.EnableSsl = true;

            mySmtpClient.UseDefaultCredentials = false;
            NetworkCredential basicAuthenticationInfo = new
           NetworkCredential("hopeshop99@outlook.com", "hopeshop78");

            mySmtpClient.Credentials = basicAuthenticationInfo;
            MailMessage message = new MailMessage();
            message.From = new MailAddress("hopeshop99@outlook.com");
            message.To.Add(new MailAddress(to.ToString()));
            string body = " Greetings from Hall Book! " + " Your book has been accepted! " ;
            message.Subject = "Success Checkout";
            message.Body = body;
            mySmtpClient.Send(message);

            return View();
        }


        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Hall)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var book = await _context.Books.FindAsync(id);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(decimal id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}

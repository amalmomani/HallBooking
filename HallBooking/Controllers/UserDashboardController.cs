using HallBooking.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
            ViewBag.Fullname = HttpContext.Session.GetString("Fullname");
            ViewBag.Userid = HttpContext.Session.GetInt32("Userid");
            ViewBag.Email = HttpContext.Session.GetString("Email");

            var holls = _context.Halls.Where(x => x.Categoryid == id);
            return View(holls);
        }


        [HttpPost]
        public async Task<IActionResult> Index(string? Index)
        {
            ViewBag.Fullname = HttpContext.Session.GetString("Fullname");
            ViewBag.Userid = HttpContext.Session.GetInt32("Userid");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            var cat = await _context.Halls.ToListAsync();
            if (Index != null)
            {
                var result = cat.Where(x => x.Hallname.ToUpper().Contains(Index.ToUpper()));
                return View(result);
            }
            return View(cat);
        }






        public async Task<IActionResult> Category()
        {

            ViewBag.Fullname = HttpContext.Session.GetString("Fullname");
            ViewBag.Userid = HttpContext.Session.GetInt32("Userid");
            ViewBag.Email = HttpContext.Session.GetString("Email");

            return View(await _context.Hallcategories.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> Category(string? category)
        {
            ViewBag.Fullname = HttpContext.Session.GetString("Fullname");
            ViewBag.Userid = HttpContext.Session.GetInt32("Userid");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            var cat = await _context.Hallcategories.ToListAsync();
            if (category != null)
            {
                var result = cat.Where(x => x.Name.ToUpper().Contains(category.ToUpper()));
                return View(result);
            }
            return View(cat);
        }


       
       




        public async Task<IActionResult> Halls()
        {

            ViewBag.Fullname = HttpContext.Session.GetString("Fullname");
            ViewBag.Userid = HttpContext.Session.GetInt32("Userid");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            var modelContext = _context.Halls.Include(h => h.Category);
            return View(await modelContext.ToListAsync());
        }

       [HttpPost]
        public async Task<IActionResult> Halls(string? halls)
        {
            ViewBag.Fullname = HttpContext.Session.GetString("Fullname");
            ViewBag.Userid = HttpContext.Session.GetInt32("Userid");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            var cat = await _context.Halls.ToListAsync();
            if (halls != null)
            {
                var result = cat.Where(x => x.Hallname.ToUpper().Contains(halls.ToUpper()));
                return View(result);
            }
            return View(cat);
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
            book.Status = "Under Process";
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return View();
        }
        public IActionResult Payment()
        {
            ViewBag.Fullname = HttpContext.Session.GetString("Fullname");
            ViewBag.Userid = HttpContext.Session.GetInt32("Userid");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Payment(decimal Cardnumber, decimal cvv)
        {
            ViewBag.Fullname = HttpContext.Session.GetString("Fullname");
            ViewBag.Userid = HttpContext.Session.GetInt32("Userid");
            ViewBag.Email = HttpContext.Session.GetString("Email"); 
            ViewBag.flag = 1;
            ViewBag.emptyorder = 1;
            ViewBag.amount = 1;
            int u = ViewBag.Userid;
            List<Book> book = _context.Books.Where(x => x.Userid == u && x.Status=="Accept").ToList();

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
                            b.Status = "Paied";
                            _context.Update(b);
                            await _context.SaveChangesAsync();
                        }
                        bank.Amount = bank.Amount - amountt;
                        _context.Update(bank);
                        await _context.SaveChangesAsync();
                        SendEmail(ViewBag.Email, amountt);
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
        public IActionResult SendEmail(string to, decimal? amount)
        {
            ViewBag.Fullname = HttpContext.Session.GetString("Fullname");
            ViewBag.Userid = HttpContext.Session.GetInt32("Userid");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            //to = ViewBag.Email;
            //MimeMessage obj = new MimeMessage();
            //MailboxAddress emailfrom = new MailboxAddress("Hall Booking", "shophope17@gmail.com");
            //MailboxAddress emailto = new MailboxAddress(ViewBag.Fullname, to);
            //obj.From.Add(emailfrom);
            //obj.To.Add(emailto);
            //obj.Subject = "Success Checkout! " + ViewBag.Fullname;
            //BodyBuilder msgbody = new BodyBuilder();
            //// bb.TextBody = body;
            //msgbody.HtmlBody = "<html>" + "<h1>" + " Greetings from Hall Booking! " + ViewBag.Fullname + "</h1>" + "</br>" + " Your bill has been paid successfully with " + "</br>" + " Total : " + amount + "$" + "</br>" + "</html>";
            //obj.Body = msgbody.ToMessageBody();
            //MailKit.Net.Smtp.SmtpClient emailclient = new MailKit.Net.Smtp.SmtpClient();
            //emailclient.Connect("smtp.gmail.com", 465, true);
            //emailclient.Authenticate("shophope17@gmail.com", "txcfhvzgekoszkbi");
            //emailclient.Send(obj);
            //emailclient.Disconnect(true);
            //emailclient.Dispose();


            string e = ViewBag.Email;
            using System.Net.Mail.SmtpClient mySmtpClient = new System.Net.Mail.SmtpClient("smtp.outlook.com", 587);
            mySmtpClient.EnableSsl = true;

            mySmtpClient.UseDefaultCredentials = false;
            NetworkCredential basicAuthenticationInfo = new
           NetworkCredential("Ahmad_bani_Yaseen@outlook.com", "@Ahmad118513");

            mySmtpClient.Credentials = basicAuthenticationInfo;
            MailMessage message = new MailMessage();
            message.From = new MailAddress("Ahmad_bani_Yaseen@outlook.com");
            message.To.Add(new MailAddress(to.ToString()));
            string body = " Greetings from Hall Book! " + ViewBag.Fullname + " Your bill has been paid successfully with" + " Total amount of " + amount + "$";
            message.Subject = "Success Checkout";
            message.Body = body;
            mySmtpClient.Send(message);

            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}

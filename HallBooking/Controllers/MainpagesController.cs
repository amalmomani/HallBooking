using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HallBooking.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace HallBooking.Controllers
{
    public class MainpagesController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment webHostEnviroment;

        public MainpagesController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            this.webHostEnviroment = webHostEnviroment;
        }

        // GET: Mainpages
        public async Task<IActionResult> Index()
        {
            return View(await _context.Mainpages.ToListAsync());
        }

        // GET: Mainpages/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mainpage = await _context.Mainpages
                .FirstOrDefaultAsync(m => m.Homeid == id);
            if (mainpage == null)
            {
                return NotFound();
            }

            return View(mainpage);
        }

        // GET: Mainpages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Mainpages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Homeid,Companylogo,Image2,Text1,Text2,Companyemail,Companyphone,ImageFile")] Mainpage mainpage)
        {
            if (ModelState.IsValid)
            {

                if (mainpage.ImageFile != null)
                {
                    string wwwrootPath = webHostEnviroment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + "_" + mainpage.ImageFile.FileName;
                    //1523f14f-5535-40c6-82bb-7d3b9edf2e75_piza2.jpg
                    string path = Path.Combine(wwwrootPath + "/Images/" + fileName);
                    using (var filestream = new FileStream(path, FileMode.Create))
                    {
                        await mainpage.ImageFile.CopyToAsync(filestream);
                    }
                    mainpage.Image2 = fileName;
                    _context.Add(mainpage);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(mainpage);
        }

        // GET: Mainpages/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mainpage = await _context.Mainpages.FindAsync(id);
            if (mainpage == null)
            {
                return NotFound();
            }
            return View(mainpage);
        }

        // POST: Mainpages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Homeid,Companylogo,Image2,Text1,Text2,Companyemail,Companyphone,ImageFile")] Mainpage mainpage)
        {
            if (id != mainpage.Homeid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (mainpage.ImageFile != null)
                    {
                        string wwwrootPath = webHostEnviroment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" + mainpage.ImageFile.FileName;
                        //1523f14f-5535-40c6-82bb-7d3b9edf2e75_piza2.jpg
                        string path = Path.Combine(wwwrootPath + "/Images/" + fileName);
                        using (var filestream = new FileStream(path, FileMode.Create))
                        {
                            await mainpage.ImageFile.CopyToAsync(filestream);
                        }
                        mainpage.Image2 = fileName;
                    }
                    _context.Update(mainpage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MainpageExists(mainpage.Homeid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(mainpage);
        }

        // GET: Mainpages/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mainpage = await _context.Mainpages
                .FirstOrDefaultAsync(m => m.Homeid == id);
            if (mainpage == null)
            {
                return NotFound();
            }

            return View(mainpage);
        }

        // POST: Mainpages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var mainpage = await _context.Mainpages.FindAsync(id);
            _context.Mainpages.Remove(mainpage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MainpageExists(decimal id)
        {
            return _context.Mainpages.Any(e => e.Homeid == id);
        }
    }
}

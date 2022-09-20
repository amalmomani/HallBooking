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
    public class HallsController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment webHostEnviroment;
        public HallsController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            this.webHostEnviroment = webHostEnviroment;
        }

        // GET: Halls
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Halls.Include(h => h.Category);
            return View(await modelContext.ToListAsync());
        }

        // GET: Halls/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hall = await _context.Halls
                .Include(h => h.Category)
                .FirstOrDefaultAsync(m => m.Hallid == id);
            if (hall == null)
            {
                return NotFound();
            }

            return View(hall);
        }

        // GET: Halls/Create
        public IActionResult Create()
        {
            ViewData["Categoryid"] = new SelectList(_context.Hallcategories, "Categoryid", "Categoryid");
            return View();
        }

        // POST: Halls/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Hallid,Hallname,Hallddress,Imagepath,Hallsize,Price,Isbooked,Categoryid,ImageFile")] Hall hall)
        {
            if (ModelState.IsValid)
            {
                if (hall.ImageFile != null)
                {
                    string wwwrootPath = webHostEnviroment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + "_" + hall.ImageFile.FileName;
                    //1523f14f-5535-40c6-82bb-7d3b9edf2e75_piza2.jpg
                    string path = Path.Combine(wwwrootPath + "/Images/" + fileName);
                    using (var filestream = new FileStream(path, FileMode.Create))
                    {
                        await hall.ImageFile.CopyToAsync(filestream);
                    }
                    hall.Imagepath = fileName;
                    _context.Add(hall);
                    await _context.SaveChangesAsync();
                }




                return RedirectToAction(nameof(Index));
            }
            ViewData["Categoryid"] = new SelectList(_context.Hallcategories, "Categoryid", "Categoryid", hall.Categoryid);
            return View(hall);
        }

        // GET: Halls/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hall = await _context.Halls.FindAsync(id);
            if (hall == null)
            {
                return NotFound();
            }
            ViewData["Categoryid"] = new SelectList(_context.Hallcategories, "Categoryid", "Categoryid", hall.Categoryid);
            return View(hall);
        }

        // POST: Halls/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Hallid,Hallname,Hallddress,Imagepath,Hallsize,Price,Isbooked,Categoryid")] Hall hall)
        {
            if (id != hall.Hallid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hall);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HallExists(hall.Hallid))
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
            ViewData["Categoryid"] = new SelectList(_context.Hallcategories, "Categoryid", "Categoryid", hall.Categoryid);
            return View(hall);
        }

        // GET: Halls/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hall = await _context.Halls
                .Include(h => h.Category)
                .FirstOrDefaultAsync(m => m.Hallid == id);
            if (hall == null)
            {
                return NotFound();
            }

            return View(hall);
        }

        // POST: Halls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var hall = await _context.Halls.FindAsync(id);
            _context.Halls.Remove(hall);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HallExists(decimal id)
        {
            return _context.Halls.Any(e => e.Hallid == id);
        }
    }
}

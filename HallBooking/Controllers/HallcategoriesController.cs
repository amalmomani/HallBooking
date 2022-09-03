using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HallBooking.Models;

namespace HallBooking.Controllers
{
    public class HallcategoriesController : Controller
    {
        private readonly ModelContext _context;

        public HallcategoriesController(ModelContext context)
        {
            _context = context;
        }

        // GET: Hallcategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Hallcategories.ToListAsync());
        }

        // GET: Hallcategories/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hallcategory = await _context.Hallcategories
                .FirstOrDefaultAsync(m => m.Categoryid == id);
            if (hallcategory == null)
            {
                return NotFound();
            }

            return View(hallcategory);
        }

        // GET: Hallcategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Hallcategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Categoryid,Name,Imagepath")] Hallcategory hallcategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hallcategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hallcategory);
        }

        // GET: Hallcategories/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hallcategory = await _context.Hallcategories.FindAsync(id);
            if (hallcategory == null)
            {
                return NotFound();
            }
            return View(hallcategory);
        }

        // POST: Hallcategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Categoryid,Name,Imagepath")] Hallcategory hallcategory)
        {
            if (id != hallcategory.Categoryid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hallcategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HallcategoryExists(hallcategory.Categoryid))
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
            return View(hallcategory);
        }

        // GET: Hallcategories/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hallcategory = await _context.Hallcategories
                .FirstOrDefaultAsync(m => m.Categoryid == id);
            if (hallcategory == null)
            {
                return NotFound();
            }

            return View(hallcategory);
        }

        // POST: Hallcategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var hallcategory = await _context.Hallcategories.FindAsync(id);
            _context.Hallcategories.Remove(hallcategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HallcategoryExists(decimal id)
        {
            return _context.Hallcategories.Any(e => e.Categoryid == id);
        }
    }
}

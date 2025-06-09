using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Booking.Data;
using Booking.Models;

namespace Booking.Controllers
{
    public class SmjestajController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SmjestajController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Smjestaj
        public async Task<IActionResult> Index()
        {
            return View(await _context.Smjestaj.ToListAsync());
        }

        // GET: Smjestaj/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var smjestaj = await _context.Smjestaj
                .FirstOrDefaultAsync(m => m.id == id);
            if (smjestaj == null)
            {
                return NotFound();
            }

            return View(smjestaj);
        }

        // GET: Smjestaj/Create
        public IActionResult Create()
        {
            ViewBag.LokacijaList = Enum.GetValues(typeof(Lokacija))
        .Cast<Lokacija>()
        .Select(u => new SelectListItem
        {
            Value = u.ToString(),
            Text = u.ToString()
        }).ToList();
            ViewBag.TipSmjestajaList = Enum.GetValues(typeof(TipSmjestaja))
        .Cast<TipSmjestaja>()
        .Select(u => new SelectListItem
        {
            Value = u.ToString(),
            Text = u.ToString()
        }).ToList();
            return View();
        }

        // POST: Smjestaj/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,idVlasnika,naziv,lokacija,tipSmjestaja,cijenaZaJednuNoc,opis,ocjena,brojSoba,imageUrl,wifi,parking,bazen,kuhinja,klima,tv,balkon")] Smjestaj smjestaj)
        {
            if (ModelState.IsValid)
            {
                _context.Add(smjestaj);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(smjestaj);
        }

        // GET: Smjestaj/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var smjestaj = await _context.Smjestaj.FindAsync(id);
            if (smjestaj == null)
            {
                return NotFound();
            }

            ViewBag.LokacijaList = Enum.GetValues(typeof(Lokacija))
    .Cast<Lokacija>()
    .Select(u => new SelectListItem
    {
        Value = u.ToString(),
        Text = u.ToString()
    }).ToList();
            ViewBag.TipSmjestajaList = Enum.GetValues(typeof(TipSmjestaja))
        .Cast<TipSmjestaja>()
        .Select(u => new SelectListItem
        {
            Value = u.ToString(),
            Text = u.ToString()
        }).ToList();
            return View(smjestaj);
        }

        // POST: Smjestaj/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,idVlasnika,naziv,lokacija,tipSmjestaja,cijenaZaJednuNoc,opis,ocjena,brojSoba,imageUrl,wifi,parking,bazen,kuhinja,klima,tv,balkon")] Smjestaj smjestaj)
        {
            if (id != smjestaj.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(smjestaj);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SmjestajExists(smjestaj.id))
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

            return View(smjestaj);
        }

        // GET: Smjestaj/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var smjestaj = await _context.Smjestaj
                .FirstOrDefaultAsync(m => m.id == id);
            if (smjestaj == null)
            {
                return NotFound();
            }

            return View(smjestaj);
        }

        // POST: Smjestaj/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var smjestaj = await _context.Smjestaj.FindAsync(id);
            if (smjestaj != null)
            {
                _context.Smjestaj.Remove(smjestaj);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SmjestajExists(int id)
        {
            return _context.Smjestaj.Any(e => e.id == id);
        }
    }
}

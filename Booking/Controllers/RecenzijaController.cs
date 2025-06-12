using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Booking.Data;
using Booking.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Booking.Controllers
{
    [Authorize]
    public class RecenzijaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Korisnik> _userManager;

        public RecenzijaController(ApplicationDbContext context, UserManager<Korisnik> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Recenzija
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Recenzija.ToListAsync());
        }

        // GET: Recenzija/Details/5
        [Authorize(Roles = "Admin, Gost")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recenzija = await _context.Recenzija
                .FirstOrDefaultAsync(m => m.id == id);
            if (recenzija == null)
            {
                return NotFound();
            }

            return View(recenzija);
        }

        // GET: Recenzija/Create
        [Authorize(Roles = "Admin, Gost")]
        public IActionResult Create(int? idSmjestaja)
        {
            var model = new Recenzija();
            if (idSmjestaja.HasValue)
            {
                model.idSmjestaja = idSmjestaja.Value;
                // Fetch nazivSmjestaja from database
                var smjestaj = _context.Smjestaj.FirstOrDefault(s => s.id == idSmjestaja.Value);
                ViewData["NazivSmjestaja"] = smjestaj?.naziv ?? "";
            }
            else
            {
                model.idSmjestaja = 0; // Default value if not provided
                ViewData["NazivSmjestaja"] = "";
            }
            model.idGosta = _userManager.GetUserId(User);
            return View(model);
        }

        // POST: Recenzija/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,idSmjestaja,idGosta,ocjena,komentar")] Recenzija recenzija)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recenzija);
                await _context.SaveChangesAsync();

                // racunamo novu prosjecnu ocjenu
                var smjestajId = recenzija.idSmjestaja;
                var recenzijeZaSmjestaj = await _context.Recenzija
                    .Where(r => r.idSmjestaja == smjestajId)
                    .ToListAsync();

                float prosjecnaOcjena = 0;
                if (recenzijeZaSmjestaj.Count > 0)
                {
                    prosjecnaOcjena = recenzijeZaSmjestaj.Average(r => r.ocjena);
                }

                var smjestaj = await _context.Smjestaj.FirstOrDefaultAsync(s => s.id == smjestajId);
                if (smjestaj != null)
                {
                    smjestaj.ocjena = prosjecnaOcjena;
                    _context.Smjestaj.Update(smjestaj);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("Index", "Home");
            }
            return View(recenzija);
        }

        // GET: Recenzija/Edit/5
        [Authorize(Roles = "Admin, Gost")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recenzija = await _context.Recenzija.FindAsync(id);
            if (recenzija == null)
            {
                return NotFound();
            }
            return View(recenzija);
        }

        // POST: Recenzija/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,idSmjestaja,idGosta,ocjena,komentar")] Recenzija recenzija)
        {
            if (id != recenzija.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recenzija);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecenzijaExists(recenzija.id))
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
            return View(recenzija);
        }

        // GET: Recenzija/Delete/5
        [Authorize(Roles = "Admin, Gost")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recenzija = await _context.Recenzija
                .FirstOrDefaultAsync(m => m.id == id);
            if (recenzija == null)
            {
                return NotFound();
            }

            return View(recenzija);
        }

        // POST: Recenzija/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recenzija = await _context.Recenzija.FindAsync(id);
            if (recenzija != null)
            {
                _context.Recenzija.Remove(recenzija);
            }

            await _context.SaveChangesAsync();
            // racunamo novu prosjecnu ocjenu
            var smjestajId = recenzija.idSmjestaja;
            var recenzijeZaSmjestaj = await _context.Recenzija
                .Where(r => r.idSmjestaja == smjestajId)
                .ToListAsync();

            float prosjecnaOcjena = 0;
            if (recenzijeZaSmjestaj.Count > 0)
            {
                prosjecnaOcjena = recenzijeZaSmjestaj.Average(r => r.ocjena);
            }

            var smjestaj = await _context.Smjestaj.FirstOrDefaultAsync(s => s.id == smjestajId);
            if (smjestaj != null)
            {
                smjestaj.ocjena = prosjecnaOcjena;
                _context.Smjestaj.Update(smjestaj);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Home");
        }

        private bool RecenzijaExists(int id)
        {
            return _context.Recenzija.Any(e => e.id == id);
        }
    }
}

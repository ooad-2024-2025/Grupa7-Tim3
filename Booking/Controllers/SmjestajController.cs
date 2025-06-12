using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Booking.Data;
using Booking.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Booking.Controllers
{
    public class SmjestajController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Korisnik> _userManager;

        public SmjestajController(ApplicationDbContext context, UserManager<Korisnik> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Smjestaj
        [Authorize(Roles ="Admin")]
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
            var recenzije = await _context.Recenzija
            .Where(r => r.idSmjestaja == smjestaj.id)
            .ToListAsync();

            var idGosti = recenzije.Select(r => r.idGosta).Distinct().ToList();

            var relevantniKorisnici = await _context.Users
                .Where(u => idGosti.Contains(u.Id))
                .ToListAsync();


            ViewData["Recenzije"] = recenzije;
            ViewData["RelevantniKorisnici"] = relevantniKorisnici;

            return View(smjestaj);
        }

        // GET: Smjestaj/Create
        [Authorize(Roles = "Admin, Ugostitelj")]
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
            var model = new Smjestaj();
            model.idVlasnika = _userManager.GetUserId(User);
            return View(model);
        }

        // POST: Smjestaj/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,idVlasnika,naziv,lokacija,adresa,tipSmjestaja,cijenaZaJednuNoc,opis,ocjena,brojSoba,imageUrl,imageUrl2,imageUrl3,wifi,parking,bazen,kuhinja,klima,tv,balkon")] Smjestaj smjestaj)
        {
            if (ModelState.IsValid)
            {

                _context.Add(smjestaj);
                await _context.SaveChangesAsync();
                return RedirectToAction( "Index", "Home");
            }

            return View(smjestaj);
        }

        // GET: Smjestaj/Edit/5
        [Authorize(Roles = "Admin,Ugostitelj")]
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
        public async Task<IActionResult> Edit(int id, [Bind("id,idVlasnika,naziv,lokacija,adresa,tipSmjestaja,cijenaZaJednuNoc,opis,ocjena,brojSoba,imageUrl,imageUrl2,imageUrl3,wifi,parking,bazen,kuhinja,klima,tv,balkon")] Smjestaj smjestaj)
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
                return RedirectToAction("Index", "Home");
            }

            return View(smjestaj);
        }

        // GET: Smjestaj/Delete/5
        [Authorize(Roles = "Admin,Ugostitelj")]
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


        [Authorize(Roles = "Admin, Ugostitelj")]
        public async Task<IActionResult> SmjestajiUgostitelja()
        {
            var idKorisnika = _userManager.GetUserId(User);
            var smjestaji = await _context.Smjestaj
                .Where(s => s.idVlasnika == idKorisnika)
                .ToListAsync();

            return View(smjestaji);
        }
    }
}

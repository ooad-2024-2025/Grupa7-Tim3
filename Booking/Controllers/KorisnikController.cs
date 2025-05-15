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
    public class KorisnikController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KorisnikController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Korisnik
        //prikaz svih dostupnih korisnika u bazi podataka
        public async Task<IActionResult> Index()
        {
            return View(await _context.Korisnik.ToListAsync());
        }

        // GET: Korisnik/Details/5
        //po id-u dobavaja korisnika iz baze i prikazuje sve podatke o njemu
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var korisnik = await _context.Korisnik
                .FirstOrDefaultAsync(m => m.id == id);
            if (korisnik == null)
            {
                return NotFound();
            }

            return View(korisnik);
        }

        // GET: Korisnik/Create
        //prikazuje pogled za DODAVANJE korisnika
        public IActionResult Create()
        {
            ViewBag.UlogaList = Enum.GetValues(typeof(Uloga))
        .Cast<Uloga>()
        .Select(u => new SelectListItem
        {
            Value = u.ToString(),
            Text = u.ToString()
        }).ToList();
            return View();
        }

        // POST: Korisnik/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        //prima korisnika, i onda ga dodaje u bazu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,ime,prezime,email,lozinka,adresa,brojTelefona,uloga")] Korisnik korisnik)
        {
            if (ModelState.IsValid)
            {
                _context.Add(korisnik);
                await _context.SaveChangesAsync();
                //Kad se doda korisnik, ide se na Index cime se prikazuju svi uneseni 
                return RedirectToAction(nameof(Index));
            }
            //ako nismo uspjeli dodat u bazu, vracamo se na view, tj tamo gdje se nalazi Create button
            return View(korisnik);
        }

        // GET: Korisnik/Edit/5
        //prikazuje ekran za editovanje korisnika pomoću id-a
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var korisnik = await _context.Korisnik.FindAsync(id);
            if (korisnik == null)
            {
                return NotFound();
            }
            ViewBag.UlogaList = Enum.GetValues(typeof(Uloga))
        .Cast<Uloga>()
        .Select(u => new SelectListItem
        {
            Value = u.ToString(),
            Text = u.ToString()
        }).ToList();
            return View(korisnik);
        }

        // POST: Korisnik/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        //ova metoda prima id korisnika i nove podatke, i vrsi njihovo updateovanaj u bazi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,ime,prezime,email,lozinka,adresa,brojTelefona,uloga")] Korisnik korisnik)
        {
            if (id != korisnik.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(korisnik);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KorisnikExists(korisnik.id))
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
            return View(korisnik);
        }

        // GET: Korisnik/Delete/5
        //prikazuje pogled za brisanje korisnika sa ovim ID-em
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var korisnik = await _context.Korisnik
                .FirstOrDefaultAsync(m => m.id == id);
            if (korisnik == null)
            {
                return NotFound();
            }

            return View(korisnik);
        }

        // POST: Korisnik/Delete/5

        //brise korisnika iz baze
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var korisnik = await _context.Korisnik.FindAsync(id);
            if (korisnik != null)
            {
                _context.Korisnik.Remove(korisnik);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //pomocna funckija koja se koristi za neke od ovih metoda
        private bool KorisnikExists(int id)
        {
            return _context.Korisnik.Any(e => e.id == id);
        }
    }
}

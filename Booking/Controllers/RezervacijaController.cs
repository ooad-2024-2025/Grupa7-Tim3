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
using Booking.Services;

namespace Booking.Controllers
{
    [Authorize]
    public class RezervacijaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Korisnik> _userManager;

        public RezervacijaController(ApplicationDbContext context, UserManager<Korisnik> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Rezervacija
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Rezervacija.ToListAsync());
        }

        // GET: Rezervacija/Details/5
        [Authorize(Roles = "Admin, Gost, Ugostitelj")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacija
                .FirstOrDefaultAsync(m => m.id == id);
            if (rezervacija == null)
            {
                return NotFound();
            }

            return View(rezervacija);
        }

        // GET: Rezervacija/Create
        [Authorize(Roles = "Admin,Gost")]
        public IActionResult Create(int? idSmjestaja)
        {

            var model = new Rezervacija();
            if (idSmjestaja.HasValue)
            {
                model.idSmjestaja = idSmjestaja.Value;
                //fetch cijenaZaJedanDan from database
                var smjestaj = _context.Smjestaj.FirstOrDefault(s => s.id == idSmjestaja.Value);
                if (smjestaj != null)
                {
                    ViewData["cijenaZaJednuNoc"] = smjestaj.cijenaZaJednuNoc;
                }
                else
                {
                    ViewData["cijenaZaJednuNoc"] = 0;
                }
            }
            model.idGosta = _userManager.GetUserId(User);
            model.datumRezervacije = DateTime.Now;
            model.datumOtkazivanja = DateTime.MinValue;
            model.pocetakBoravka = DateTime.Now;
            model.krajBoravka = DateTime.Now.AddDays(1);
            model.rezervacijaOtkazana = false;


            return View(model);
        }

        // POST: Rezervacija/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,idSmjestaja,idGosta,datumRezervacije,pocetakBoravka,krajBoravka,cijena,rezervacijaOtkazana,datumOtkazivanja")] Rezervacija rezervacija)
        {
            if (ModelState.IsValid && rezervacija.pocetakBoravka < rezervacija.krajBoravka && (rezervacija.krajBoravka - rezervacija.pocetakBoravka).TotalDays >= 1)
            {
                _context.Add(rezervacija);
                await _context.SaveChangesAsync();
                // Send email notification
                var user = await _userManager.GetUserAsync(User);
                var email = user?.Email;

                if (!string.IsNullOrEmpty(email))
                {
                    var smjestaj = await _context.Smjestaj.FirstOrDefaultAsync(s => s.id == rezervacija.idSmjestaja);
                    var brojNocenja = (rezervacija.krajBoravka - rezervacija.pocetakBoravka).Days;

                    var htmlContent = $@"
                <h2>Potvrda rezervacije</h2>
                <p>Poštovani,</p>
                <p>Vaša rezervacija je uspješno izvršena. U nastavku su detalji:</p>
                <ul>
                    <li><strong>Smještaj:</strong> {smjestaj?.naziv}</li>
                    <li><strong>Početak boravka:</strong> {rezervacija.pocetakBoravka:dd.MM.yyyy}</li>
                    <li><strong>Kraj boravka:</strong> {rezervacija.krajBoravka:dd.MM.yyyy}</li>
                    <li><strong>Broj noćenja:</strong> {brojNocenja}</li>
                    <li><strong>Ukupna cijena:</strong> {rezervacija.cijena:0.00} KM</li>
                </ul>
                <p>Hvala što koristite Booking.com!</p>
            ";
                    var emailService = new EmailService();
                    await emailService.SendEmailAsync(email, htmlContent);
                }
                return View("UspjesnaRezervacija");
            }
            return View(rezervacija);
        }

        // GET: Rezervacija/Edit/5
        [Authorize(Roles = "Admin, Gost")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacija.FindAsync(id);
            if (rezervacija == null)
            {
                return NotFound();
            }
            return View(rezervacija);
        }

        // POST: Rezervacija/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,idSmjestaja,idGosta,datumRezervacije,pocetakBoravka,krajBoravka,cijena,rezervacijaOtkazana,datumOtkazivanja")] Rezervacija rezervacija)
        {
            if (id != rezervacija.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    rezervacija.datumOtkazivanja = DateTime.Now;
                    rezervacija.rezervacijaOtkazana = true;
                    _context.Update(rezervacija);
                    await _context.SaveChangesAsync();

                    // Dohvati korisnika
                    var korisnik = await _context.Users.FindAsync(rezervacija.idGosta);

                    // Dohvati smještaj
                    var smjestaj = await _context.Smjestaj.FindAsync(rezervacija.idSmjestaja);

                    if (korisnik != null && smjestaj != null && !string.IsNullOrEmpty(korisnik.Email))
                    {
                        var emailService = new EmailService();

                        var htmlContent = $@"
                    <p><strong>Vaša rezervacija je otkazana.</strong></p>
                    <p>Smještaj: <strong>{smjestaj.naziv}</strong></p>
                    <p>Period: {rezervacija.pocetakBoravka:dd.MM.yyyy.} - {rezervacija.krajBoravka:dd.MM.yyyy.}</p>
                    <p>Cijena: {rezervacija.cijena} KM</p>
                    <p>Status: <span style='color:red;'><strong>OTKAZANO</strong></span></p>
                ";

                        await emailService.SendEmailAsync(korisnik.Email, htmlContent);
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RezervacijaExists(rezervacija.id))
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
            return View(rezervacija);
        }

        // GET: Rezervacija/Delete/5
        [Authorize(Roles = "Admin, Gost, Ugostitelj")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacija
                .FirstOrDefaultAsync(m => m.id == id);
            if (rezervacija == null)
            {
                return NotFound();
            }

            return View(rezervacija);
        }

        // POST: Rezervacija/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rezervacija = await _context.Rezervacija.FindAsync(id);
            if (rezervacija != null)
            {
                _context.Rezervacija.Remove(rezervacija);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RezervacijaExists(int id)
        {
            return _context.Rezervacija.Any(e => e.id == id);
        }
        [Authorize(Roles = "Admin, Ugostitelj")]
        public async Task<IActionResult> RezervacijeZaSmjestaj(int idSmjestaja)
        {
            var rezervacije = await _context.Rezervacija
                .Where(r => r.idSmjestaja == idSmjestaja)
                .ToListAsync();

            var gostIds = rezervacije.Select(r => r.idGosta).Distinct().ToList();

            var relevantniKorisnici = await _context.Users
                .Where(u => gostIds.Contains(u.Id))
                .ToListAsync();

            var smjestaj = await _context.Smjestaj
                .FirstOrDefaultAsync(s => s.id == idSmjestaja);

            ViewBag.NazivSmjestaja = smjestaj?.naziv;
            ViewBag.RelevantniKorisnici = relevantniKorisnici;


            return View(rezervacije);
        }
        [Authorize(Roles = "Admin, Gost")]
        public async Task<IActionResult> RezervacijeGosta()
        {
            var idGosta = _userManager.GetUserId(User);
            var rezervacije = await _context.Rezervacija
                .Where(r => r.idGosta== idGosta)
                .ToListAsync();

            var smjestajIds = rezervacije.Select(r => r.idSmjestaja).Distinct().ToList();

            var relevantniSmjestaji = await _context.Smjestaj
                .Where(s => smjestajIds.Contains(s.id))
                .ToListAsync();
            ViewBag.RelevantniSmjestaji = relevantniSmjestaji;


            return View(rezervacije);
        }

        public async Task<IActionResult> PosaljiMail()
        {
            //var emailService = new EmailService();
            //await emailService.SendEmailAsync();

            return RedirectToAction("Index", "Home");
        }


    }
}

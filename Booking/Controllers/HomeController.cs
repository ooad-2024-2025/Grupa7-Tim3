using System.Diagnostics;
using Booking.Models;
using Microsoft.AspNetCore.Mvc;
using Booking.Data; 
using Microsoft.EntityFrameworkCore; 
using System.Threading.Tasks; 
using System.Linq; 
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Booking.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Korisnik> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<Korisnik> userManager)
        {
            _logger = logger;
            _context = context; 
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(
            string searchQuery,
        TipSmjestaja? tipSmjestaja,
        Lokacija? lokacija,
        float? minCijena,
        float? maxCijena)
        {
            List<Smjestaj> smjestaji;

            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                string gostId = user.Id;

                var rezervacije = await _context.Rezervacija
                    .Where(r => r.idGosta == gostId)
                    .ToListAsync();
                //uzimamo znaci id svakog smjestaja kojeg je korisnik ranije rezervisao
                var smjestajIds = rezervacije.Select(r => r.idSmjestaja).Distinct().ToList();

                var prethodniSmjestaji = await _context.Smjestaj
                    .Where(s => smjestajIds.Contains(s.id))
                    .ToListAsync();

                if (prethodniSmjestaji.Any())
                {
                    float prosjecnaCijena = prethodniSmjestaji.Average(s => s.cijenaZaJednuNoc);
                    var tipovi = prethodniSmjestaji.Select(s => s.tipSmjestaja).Distinct().ToList();
                    var lokacije = prethodniSmjestaji.Select(s => s.lokacija).Distinct().ToList();

                    var sviSmjestaji = await _context.Smjestaj.ToListAsync();

                    smjestaji = sviSmjestaji
                        .Select(s => new
                        {
                            Smjestaj = s,
                            RazlikaUCijeni = Math.Abs(s.cijenaZaJednuNoc - prosjecnaCijena),
                            PoklapanjeTipa = tipovi.Contains(s.tipSmjestaja) ? 0 : 1,
                            PoklapanjeLokacije = lokacije.Contains(s.lokacija) ? 0 : 1
                        })
                        .OrderBy(x => x.RazlikaUCijeni)
                        .ThenBy(x => x.PoklapanjeTipa)
                        .ThenBy(x => x.PoklapanjeLokacije)
                        .Select(x => x.Smjestaj)
                        .ToList();
                }
                else
                {
                    smjestaji = await _context.Smjestaj.ToListAsync();
                }
            }
            else
            {
                smjestaji = await _context.Smjestaj.ToListAsync();
            }

            // primjena dodatnih filtera ako su uneseni
            if (!string.IsNullOrWhiteSpace(searchQuery))
                smjestaji = smjestaji.Where(s => s.naziv.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)).ToList();

            if (tipSmjestaja.HasValue)
                smjestaji = smjestaji.Where(s => s.tipSmjestaja == tipSmjestaja.Value).ToList();

            if (lokacija.HasValue)
                smjestaji = smjestaji.Where(s => s.lokacija == lokacija.Value).ToList();

            if (minCijena.HasValue)
                smjestaji = smjestaji.Where(s => s.cijenaZaJednuNoc >= minCijena.Value).ToList();

            if (maxCijena.HasValue)
                smjestaji = smjestaji.Where(s => s.cijenaZaJednuNoc <= maxCijena.Value).ToList();

            // Postavi sve u ViewData
            ViewData["Smjestaji"] = smjestaji;
            ViewData["SearchQuery"] = searchQuery;
            ViewData["TipSmjestaja"] = tipSmjestaja;
            ViewData["Lokacija"] = lokacija;
            ViewData["MinCijena"] = minCijena;
            ViewData["MaxCijena"] = maxCijena;
            ViewData["Tipovi"] = Enum.GetValues(typeof(TipSmjestaja)).Cast<TipSmjestaja>().ToList();
            ViewData["Lokacije"] = Enum.GetValues(typeof(Lokacija)).Cast<Lokacija>().ToList();

            return View();
        }

        /*
        public IActionResult Index()
        {
            return View();
        }
        */
        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }










    }
}

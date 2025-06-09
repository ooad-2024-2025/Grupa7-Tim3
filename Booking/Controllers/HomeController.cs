using System.Diagnostics;
using Booking.Models;
using Microsoft.AspNetCore.Mvc;
using Booking.Data; // dodano
using Microsoft.EntityFrameworkCore; // dodano
using System.Threading.Tasks; // dodano
using System.Linq; // dodano
using System.Collections.Generic; // dodano

namespace Booking.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context; // dodano
        }
        public async Task<IActionResult> Index(
            string searchQuery,
            TipSmjestaja? tipSmjestaja,
            Lokacija? lokacija,
            float? minCijena,
            float? maxCijena)
        {
            var query = _context.Smjestaj.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
                query = query.Where(s => s.naziv.Contains(searchQuery));

            if (tipSmjestaja.HasValue)
                query = query.Where(s => s.tipSmjestaja == tipSmjestaja.Value);

            if (lokacija.HasValue)
                query = query.Where(s => s.lokacija == lokacija.Value);

            if (minCijena.HasValue)
                query = query.Where(s => s.cijenaZaJednuNoc >= minCijena.Value);

            if (maxCijena.HasValue)
                query = query.Where(s => s.cijenaZaJednuNoc <= maxCijena.Value);

            var smjestaji = await query.ToListAsync();

            ViewData["Smjestaji"] = smjestaji;
            ViewData["SearchQuery"] = searchQuery;
            ViewData["TipSmjestaja"] = tipSmjestaja;
            ViewData["Lokacija"] = lokacija;
            ViewData["MinCijena"] = minCijena;
            ViewData["MaxCijena"] = maxCijena;
            ViewData["Tipovi"] = System.Enum.GetValues(typeof(TipSmjestaja)).Cast<TipSmjestaja>().ToList();
            ViewData["Lokacije"] = System.Enum.GetValues(typeof(Lokacija)).Cast<Lokacija>().ToList();

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

using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Booking.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Booking.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<Korisnik> _userManager;
        private readonly SignInManager<Korisnik> _signInManager;

        public IndexModel(UserManager<Korisnik> userManager, SignInManager<Korisnik> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Email")]
            public string Email { get; set; }

            [RegularExpression(@"^[A-ZščćžđČŠĆŽĐa-z\s\-]{2,30}$", ErrorMessage = "Ime smije sadržavati samo slova i razmake.")]
            [Display(Name = "Ime")]
            public string Ime { get; set; }

            [RegularExpression(@"^[A-ZščćžđČŠĆŽĐa-z\s\-]{2,30}$", ErrorMessage = "Prezime smije sadržavati samo slova i razmake.")]
            [Display(Name = "Prezime")]
            public string Prezime { get; set; }

            [Display(Name = "Adresa")]
            public string Adresa { get; set; }

            [RegularExpression(@"^\d{9,}$", ErrorMessage = "Telefon mora imati najmanje 9 cifara i bez slova.")]
            [Display(Name = "Telefon")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(Korisnik user)
        {
            Username = await _userManager.GetUserNameAsync(user);
            var email = await _userManager.GetEmailAsync(user);

            Input = new InputModel
            {
                Email = email,
                Ime = user.ime,
                Prezime = user.prezime,
                Adresa = user.adresa,
                PhoneNumber = user.brojTelefona
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            user.ime = Input.Ime;
            user.prezime = Input.Prezime;
            user.adresa = Input.Adresa;
            user.brojTelefona = Input.PhoneNumber;

            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);

            StatusMessage = "Profil je uspješno ažuriran.";
            return RedirectToPage();
        }
    }
}

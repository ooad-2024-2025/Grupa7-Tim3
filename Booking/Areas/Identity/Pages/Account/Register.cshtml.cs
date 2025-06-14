using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Booking.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace Booking.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<Korisnik> _signInManager;
        private readonly UserManager<Korisnik> _userManager;
        private readonly IUserStore<Korisnik> _userStore;
        private readonly IUserEmailStore<Korisnik> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<Korisnik> userManager,
            IUserStore<Korisnik> userStore,
            SignInManager<Korisnik> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        // 👇 Dodaj listu uloga za dropdown
        public List<Uloga> SveUloge { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Ime je obavezno.")]
            [RegularExpression(@"^[A-ZščćžđČŠĆŽĐa-z\s\-]{2,30}$", ErrorMessage = "Ime smije sadržavati samo slova i razmake.")]
            [Display(Name = "Ime")]
            public string Ime { get; set; }

            [Required(ErrorMessage = "Prezime je obavezno.")]
            [RegularExpression(@"^[A-ZčšćžđČŠĆŽĐa-z\s\-]{2,30}$", ErrorMessage = "Prezime smije sadržavati samo slova i razmake.")]
            [Display(Name = "Prezime")]
            public string Prezime { get; set; }

            [Required(ErrorMessage = "Adresa je obavezna.")]
            [Display(Name = "Adresa")]
            public string Adresa { get; set; }

            [Required(ErrorMessage = "Broj telefona je obavezan.")]
            [RegularExpression(@"^\d{9,}$", ErrorMessage = "Broj telefona mora imati najmanje 9 cifara i ne smije sadržavati slova.")]
            [Display(Name = "Broj telefona")]
            public string BrojTelefona { get; set; }


            [Required(ErrorMessage = "Email je obavezan.")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Lozinka je obavezna.")]
            [StringLength(100, ErrorMessage = "Lozinka mora imati najmanje {2}, a najviše {1} karaktera.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Lozinka")]
            public string Password { get; set; }

            [Required(ErrorMessage = "Potvrdi lozinku polje je obavezno.")]
            [DataType(DataType.Password)]
            [Display(Name = "Potvrdi lozinku")]
            [Compare("Password", ErrorMessage = "Lozinka i potvrda lozinke se ne podudaraju.")]
            public string ConfirmPassword { get; set; }

            
            [Required(ErrorMessage = "Odaberite ulogu!")]
            [Display(Name = "Uloga")]
            public Uloga? Uloga { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            SveUloge = Enum.GetValues(typeof(Uloga)).Cast<Uloga>().ToList(); // 👈 punjenje enum liste
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            SveUloge = Enum.GetValues(typeof(Uloga)).Cast<Uloga>().ToList(); // 👈 ponovno punjenje ako forma nije validna

            if (ModelState.IsValid)
            {
                var user = CreateUser();

                user.ime = Input.Ime;
                user.prezime = Input.Prezime;
                user.adresa = Input.Adresa;
                user.brojTelefona = Input.BrojTelefona;
                user.uloga = Input.Uloga; // 👈 snimi ulogu

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    await _userManager.AddToRoleAsync(user, Input.Uloga.ToString());

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Potvrdite svoj račun",
                        $"Molimo potvrdite svoj račun klikom <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>ovdje</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Ako forma nije validna
            return Page();
        }

        private Korisnik CreateUser()
        {
            try
            {
                return Activator.CreateInstance<Korisnik>();
            }
            catch
            {
                throw new InvalidOperationException($"Nije moguće kreirati instancu '{nameof(Korisnik)}'. " +
                    $"Provjeri da '{nameof(Korisnik)}' nije apstraktna klasa i da ima konstruktor bez parametara.");
            }
        }

        private IUserEmailStore<Korisnik> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
                throw new NotSupportedException("Email nije podržan u trenutnoj konfiguraciji.");

            return (IUserEmailStore<Korisnik>)_userStore;
        }
    }
}

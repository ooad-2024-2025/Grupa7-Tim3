using Microsoft.AspNetCore.Identity;
namespace Booking.Models
{
    public class Korisnik : IdentityUser
    {
        public string? ime { get; set; }
        public string? prezime { get; set; } 
        public string? lozinka { get; set; }
        public string? adresa { get; set; }
        public string? brojTelefona { get; set; }
        public Uloga? uloga { get; set; }
       
    }
}

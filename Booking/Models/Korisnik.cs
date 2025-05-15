namespace Booking.Models
{
    public class Korisnik
    {
        public int id { get; set; }
        public int ime { get; set; }
        public int prezime { get; set; }
        public int email { get; set; }
        public int lozinka { get; set; }
        public int adresa { get; set; }
        public int brojTelefona { get; set; }
        public Uloga uloga { get; set; }
       
    }
}

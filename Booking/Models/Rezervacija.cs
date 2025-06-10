namespace Booking.Models
{

    public class Rezervacija
    {
        public int id { get; set; }
        public int idSmjestaja { get; set; }
        public string idGosta { get; set; }
        public DateTime datumRezervacije { get; set; }
        public DateTime pocetakBoravka { get; set; }
        public DateTime krajBoravka { get; set; }
        public float cijena { get; set; }
        public bool rezervacijaOtkazana { get; set; }
        public DateTime datumOtkazivanja { get; set; }

    }
}

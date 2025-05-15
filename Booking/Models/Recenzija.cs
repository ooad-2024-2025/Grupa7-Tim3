namespace Booking.Models
{
    public class Recenzija
    {
        public int id { get; set; }
        public int idSmjestaja { get; set; }
        public int idGosta { get; set; }
        public float ocjena { get; set; }
        public string komentar { get; set; }

    }
}

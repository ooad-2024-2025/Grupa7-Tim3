namespace Booking.Models
{
    public class Smjestaj
    {
        public int id { get; set; }
        public int idVlasnika { get; set; }
        public string naziv { get; set; }
        public Lokacija lokacija { get; set; }
        public TipSmjestaja tipSmjestaja { get; set; }
        public float cijenaZaJednuNoc { get; set; }
        public string opis { get; set; }
    }
}

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
        public float ocjena { get; set; }
        public int brojSoba { get; set; }
        public string imageUrl { get; set; } // URL slike
        public bool wifi { get; set; }
        public bool parking { get; set; }
        public bool bazen { get; set; }
        public bool kuhinja { get; set; }
        public bool klima { get; set; }
        public bool tv { get; set; }
        public bool balkon { get; set; }

    }
}

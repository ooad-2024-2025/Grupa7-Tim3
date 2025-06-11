using System.ComponentModel.DataAnnotations;

namespace Booking.Models
{

public class Recenzija
    {
        public int id { get; set; }
        public int idSmjestaja { get; set; }
        public string idGosta { get; set; }
        [Range(0, 5, ErrorMessage = "Ocjena mora biti između 0 i 5.")]
        public float ocjena { get; set; }
        [MaxLength(300, ErrorMessage = "Komentar ne smije imati više od 300 karaktera.")]
        public string komentar { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace Booking.Models
{
    public enum TipSmjestaja
    {
        [Display(Name = "Vila")] Vila,
        [Display(Name = "Stan")] Stan,
        [Display(Name = "Kuća")] Kuca,
        [Display(Name = "Vikendica")] Vikendica,
        [Display(Name = "Apartman")] Apartman
    }
}

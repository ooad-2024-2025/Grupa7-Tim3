using System.ComponentModel.DataAnnotations;

namespace Booking.Models
{
    public enum Lokacija
    {
        [Display(Name = "Sarajevo")]
        Sarajevo,

        [Display(Name = "Banja Luka")]
        BanjaLuka,

        [Display(Name = "Tuzla")]
        Tuzla,

        [Display(Name = "Zenica")]
        Zenica,

        [Display(Name = "Mostar")]
        Mostar,

        [Display(Name = "Bihać")]
        Bihac,

        [Display(Name = "Brčko")]
        Brcko,

        [Display(Name = "Prijedor")]
        Prijedor,

        [Display(Name = "Bijeljina")]
        Bijeljina,

        [Display(Name = "Trebinje")]
        Trebinje,

        [Display(Name = "Doboj")]
        Doboj,

        [Display(Name = "Travnik")]
        Travnik,

        [Display(Name = "Cazin")]
        Cazin,

        [Display(Name = "Goražde")]
        Gorazde,

        [Display(Name = "Gračanica")]
        Gracanica,

        [Display(Name = "Gradačac")]
        Gradacac,

        [Display(Name = "Široki Brijeg")]
        SirokiBrijeg,

        [Display(Name = "Zavidovići")]
        Zavidovici,

        [Display(Name = "Lukavac")]
        Lukavac,

        [Display(Name = "Konjic")]
        Konjic,

        [Display(Name = "Sanski Most")]
        SanskiMost,

        [Display(Name = "Kakanj")]
        Kakanj,

        [Display(Name = "Bugojno")]
        Bugojno,

        [Display(Name = "Derventa")]
        Derventa,

        [Display(Name = "Zvornik")]
        Zvornik,

        [Display(Name = "Tešanj")]
        Tesanj,

        [Display(Name = "Livno")]
        Livno,

        [Display(Name = "Čapljina")]
        Capljina,

        [Display(Name = "Foča")]
        Foca,

        [Display(Name = "Modriča")]
        Modrica
    }
}

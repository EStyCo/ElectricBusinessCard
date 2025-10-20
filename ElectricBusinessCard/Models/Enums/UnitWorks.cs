using System.ComponentModel.DataAnnotations;

namespace ElectricBusinessCard.Models.Enums
{
    public enum UnitWorks
    {
        [Display(Name = "штука")]
        Piece,

        [Display(Name = "кв. метр")]
        SquareMeter,

        [Display(Name = "куб. метр")]
        CubicMeter
    }
}

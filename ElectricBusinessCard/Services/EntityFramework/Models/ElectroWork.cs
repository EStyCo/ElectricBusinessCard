using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ElectricBusinessCard.Models.Enums;

namespace ElectricBusinessCard.Services.EntityFramework.Models
{
    public class ElectroWork
    {
        [Column(Order = 1)]
        public int Id { get; set; }

        [Column(Order = 2)]
        public int WorkIndex { get; set; }

        [Column(Order = 3)]
        public int CategoryId { get; set; }

        public CategoryWork Category { get; set; }

        [Required(ErrorMessage = "Укажите название работы")]
        [Display(Name = "Название работы")]
        [MaxLength(100)]
        [Column(Order = 4)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Выберите ед. измерения")]
        [Display(Name = "Ед. измерения")]
        [Column(Order = 5)]
        public UnitWorks Unit { get; set; }

        [Required(ErrorMessage = "Укажите цену")]
        [Display(Name = "Цена (руб)")]
        [Range(0, 1000000, ErrorMessage = "Цена должна быть положительной")]
        [Column(Order = 6)]
        public int PriceInRubles { get; set; }

        [Display(Name = "Описание")]
        [Column(Order = 7)]
        public string? Description { get; set; }
    }
}
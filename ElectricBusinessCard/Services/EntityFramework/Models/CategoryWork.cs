using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ElectricBusinessCard.Models;

namespace ElectricBusinessCard.Services.EntityFramework.Models
{
    public class CategoryWork
    {
        [Column(Order = 1)]
        public int Id { get; set; }

        [Column(Order = 2)]
        public int CategoryIndex { get; set; }

        [Column(Order = 3)]
        public string Name { get; set; }

        [Column(Order = 4)]
        public string? Description { get; set; }

        [Column(Order = 5)]
        public ICollection<ElectroWork> Works { get; set; } 
    }
}

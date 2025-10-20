using System.ComponentModel.DataAnnotations;

namespace ElectricBusinessCard.Models
{
    public class TelegramMessage
    {
        [Required(ErrorMessage = "Пожалуйста, введите ваше имя")]
        [StringLength(20, MinimumLength = 2,
        ErrorMessage = "Имя должно содержать от 2 до 20 символов")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите ваш телефон")]
        [Phone]
        public string Phone { get; set; }

        [StringLength(150, ErrorMessage = "Сообщение не должно превышать 150 символов")]
        public string? Message { get; set; }
    }
}

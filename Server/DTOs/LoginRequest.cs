using System.ComponentModel.DataAnnotations;
namespace LgpdApp.Server.DTOs
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email обязателен.")]
        [EmailAddress(ErrorMessage = "Некорректный формат Email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль обязателен.")]
        public string Password { get; set; }
    }
}

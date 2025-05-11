using System.ComponentModel.DataAnnotations;

namespace LgpdApp.Server.DTOs
{
    public class CreateGameRequest
    {
        [Required(ErrorMessage = "Название игры обязательно.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "ID шаблона обязателен.")]
        public Guid TemplateId { get; set; }

        [Required(ErrorMessage = "Параметры игры обязательны.")]
        public Dictionary<string, string> Parameters { get; set; }
    }
}
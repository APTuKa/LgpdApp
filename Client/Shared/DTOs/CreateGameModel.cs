using System.ComponentModel.DataAnnotations;

namespace LgpdApp.Client.Shared.DTOs
{
    public class CreateGameModel
    {
        [Required(ErrorMessage = "Название игры обязательно.")]
        public string Name { get; set; } = "";

        public Dictionary<string, string> Parameters { get; set; } = new();
    }
}

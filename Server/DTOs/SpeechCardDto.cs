using System;
using System.ComponentModel.DataAnnotations;

namespace LgpdApp.Server.DTOs
{
    public class SpeechCardDto
    {
        [Required(ErrorMessage = "ID ребенка обязателен.")]
        public Guid ChildId { get; set; }

        [Required(ErrorMessage = "Данные речевой карты обязательны.")]
        public string Data { get; set; }
    }
}

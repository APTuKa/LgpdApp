using System.Collections.Generic;
namespace LgpdApp.Server.Models
{
    public class Game
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid TemplateId { get; set; }
        public Guid CreatedBy { get; set; }
        public string ParamsJson { get; set; } // Параметры игры в JSON формате
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Template Template { get; set; }
        public User Creator { get; set; }
        public ICollection<Card> Cards { get; set; }
    }
}

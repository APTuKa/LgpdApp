using System.Collections.Generic;
namespace LgpdApp.Client.Shared.DTOs
{
    public class CreateGameRequest
    {
        public string Name { get; set; }
        public Guid TemplateId { get; set; }
        public Dictionary<string, string> Parameters { get; set; } = new();
    }
}

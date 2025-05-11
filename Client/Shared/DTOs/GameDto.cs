using System.Text.Json;
using System.Text.Json.Serialization;
namespace LgpdApp.Client.Shared.DTOs
{
    public class GameDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ParamsJson { get; set; }
        public Guid TemplateId { get; set; }
        public TemplateDto Template { get; set; }
        public DateTime CreatedAt { get; set; }

        [JsonIgnore]
        public object ParsedParams => string.IsNullOrEmpty(ParamsJson) ? null : JsonDocument.Parse(ParamsJson);
    }
}

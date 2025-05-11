using System.Text.Json;
using System.Text.Json.Serialization;

namespace LgpdApp.Server.DTOs
{
    public class GameDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string TemplateName { get; set; }
        public string ParamsJson { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        [JsonIgnore]
        public object ParsedParams => string.IsNullOrEmpty(ParamsJson) ? null : JsonDocument.Parse(ParamsJson);
    }
}
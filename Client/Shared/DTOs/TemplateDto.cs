using System.Collections.Generic;

namespace LgpdApp.Client.Shared.DTOs
{
    public class TemplateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<TemplateParam> Parameters { get; set; } = new();
    }

    public class TemplateParam
    {
        public string Name { get; set; }
        public string Type { get; set; } // enum, int, bool, imageOrColor, list, object
        public List<string> Options { get; set; } = new();
        public bool Required { get; set; }
        public string Description { get; set; }
    }
}

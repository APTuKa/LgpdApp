using System.Collections.Generic;
namespace LgpdApp.Server.DTOs
{
    public class TemplateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<TemplateParam> Parameters { get; set; }
    }
    public class TemplateParam
    {
        public string Name { get; set; }
        public string Type { get; set; } // string, int, enum и т.д.
        public List<string> Options { get; set; } // если это выбор из списка
    }
}

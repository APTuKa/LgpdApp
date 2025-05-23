﻿using System.Collections.Generic;
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
        public string Type { get; set; } // enum: "string", "int", "bool", "imageOrColor", "list", "object"
        public string Description { get; set; }
        public List<string> Options { get; set; } // для enum
        public bool Required { get; set; }
    }
}

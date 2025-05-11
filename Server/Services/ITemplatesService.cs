using System.Collections.Generic;
using LgpdApp.Server.DTOs;

namespace LgpdApp.Server.Services
{
    public interface ITemplatesService
    {
        List<TemplateDto> GetAllTemplates();
        TemplateDto GetTemplateById(Guid id);
    }
}
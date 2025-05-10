using LgpdApp.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace LgpdApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TemplatesController : ControllerBase
    {
        private readonly TemplatesService _templatesService;

        public TemplatesController(TemplatesService templatesService)
        {
            _templatesService = templatesService;
        }

        [HttpGet]
        public IActionResult GetTemplates()
        {
            var templates = _templatesService.GetAllTemplates();
            return Ok(templates);
        }

        [HttpGet("{id}")]
        public IActionResult GetTemplate(Guid id)
        {
            var template = _templatesService.GetTemplateById(id);
            if (template == null)
                return NotFound();

            return Ok(template);
        }
    }
}

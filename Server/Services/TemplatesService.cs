using LgpdApp.Server.DTOs;
namespace LgpdApp.Server.Services
{
    public class TemplatesService
    {
        private readonly List<TemplateDto> _templates;
        public TemplatesService()
        {
            _templates = new List<TemplateDto>
            {
                
                new TemplateDto
                {
                  Id = Guid.NewGuid(),
                  Name = "Найди пару",
                  Description = "Игра на поиск пар одинаковых карточек или смысловых пар. Поддерживает два режима: Мемори и Смысловые пары.",
                  Parameters = new List<TemplateParam>
                  {
                      new TemplateParam
                      {
                         Name = "Режим игры",
                          Type = "enum",
                           Options = new List<string> { "Memori", "SemanticPairs" }
                       },
                      new TemplateParam
                      {
                        Name = "Размер поля",
                        Type = "enum",
                        Options = new List<string> { "3x2", "3x3", "3x4", "4x4", "4x5" }
                        },
                      new TemplateParam
                      {
                        Name = "Фон поля",
                        Type = "image",
                        Options = new List<string>()
                      },
                      new TemplateParam
                      {
                        Name = "Фон карточки",
                        Type = "imageOrColor",
                        Options = new List<string>()
                      },
                      new TemplateParam
                      {
                        Name = "Карточки/Пары",
                        Type = "list",
                        Options = new List<string> { "left", "right", "isPair", "type", "content" }
                      }
                  }
                }

            };
        }

        public List<TemplateDto> GetAllTemplates() => _templates;

        public TemplateDto GetTemplateById(Guid id) => _templates.FirstOrDefault(t => t.Id == id);
    }
}

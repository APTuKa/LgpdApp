using LgpdApp.Server.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace LgpdApp.Server.Services
{
    public class TemplatesService : ITemplatesService
    {
        private readonly List<TemplateDto> _templates;

        public TemplatesService()
        {
            _templates = new List<TemplateDto>
            {
                // Шаблон 1: Найди пару
                new TemplateDto
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    Name = "Найди пару",
                    Description = "Игра на поиск одинаковых карточек или смысловых пар.",
                    Parameters = new List<TemplateParam>
                    {
                        new TemplateParam {
                            Name = "Режим игры",
                            Type = "enum",
                            Options = new List<string> { "Memori", "SemanticPairs" },
                            Required = true,
                            Description = "Режим игры: классический мемори или смысловые пары."
                        },
                        new TemplateParam {
                            Name = "Размер поля",
                            Type = "enum",
                            Options = new List<string> { "3x2", "3x3", "3x4", "4x4", "4x5" },
                            Required = true,
                            Description = "Количество строк и столбцов игрового поля."
                        },
                        new TemplateParam {
                            Name = "Фон поля",
                            Type = "imageOrColor",
                            Required = false,
                            Description = "Цвет или изображение фона игрового поля."
                        },
                        new TemplateParam {
                            Name = "Фон карточек",
                            Type = "imageOrColor",
                            Required = false,
                            Description = "Цвет или изображение рубашки карточки."
                        },
                        new TemplateParam {
                            Name = "Карточки/Пары",
                            Type = "list",
                            Required = true,
                            Description = "Список парных карточек с текстом или картинкой."
                        }
                    }
                },

                // Шаблон 2: Выбери вариант ответа
                new TemplateDto
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                    Name = "Выбери вариант ответа",
                    Description = "Игра типа теста: показывается задание/вопрос, варианты ответов от 2 до 5.",
                    Parameters = new List<TemplateParam>
                    {
                        new TemplateParam {
                            Name = "Кол-во вариантов ответа",
                            Type = "enum",
                            Options = new List<string> { "2", "3", "4", "5" },
                            Required = true,
                            Description = "Сколько вариантов у каждого вопроса"
                        },
                        new TemplateParam {
                            Name = "Кол-во тестовых заданий",
                            Type = "int",
                            Required = true,
                            Description = "Сколько вопросов будет в одной игре"
                        },
                        new TemplateParam {
                            Name = "Варианты ответов",
                            Type = "list",
                            Description = "Список ответов: текст или картинка + флаг правильности"
                        },
                        new TemplateParam {
                            Name = "Фон задания",
                            Type = "imageOrColor",
                            Description = "Настройка фона задания: цвет или изображение"
                        },
                        new TemplateParam {
                            Name = "Режим множественного выбора",
                            Type = "bool",
                            Description = "Можно ли выбрать несколько правильных вариантов"
                        }
                    }
                },

                // Шаблон 3: Лопни пузырь
                new TemplateDto
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                    Name = "Лопни пузырь",
                    Description = "Игра, в которой в центре экрана находится главный объект, а вокруг него — пузыри с вариантами ответов.",
                    Parameters = new List<TemplateParam>
                    {
                        new TemplateParam {
                            Name = "Центральный объект",
                            Type = "object",
                            Description = "Что находится по центру игрового поля: текст или картинка"
                        },
                        new TemplateParam {
                            Name = "Пузыри",
                            Type = "list",
                            Description = "Список пузырей: текст или картинка + флаг правильности"
                        },
                        new TemplateParam {
                            Name = "Фон поля",
                            Type = "imageOrColor",
                            Description = "Настройка фона игрового поля: цвет или изображение"
                        },
                        new TemplateParam {
                            Name = "Количество листов",
                            Type = "int",
                            Options = Enumerable.Range(1, 10).Select(i => i.ToString()).ToList(),
                            Description = "Сколько страниц/вопросов будет в одной игре"
                        },
                        new TemplateParam {
                            Name = "Режим множественного выбора",
                            Type = "bool",
                            Description = "Можно ли выбрать несколько правильных вариантов"
                        }
                    }
                }
            };
        }

        public List<TemplateDto> GetAllTemplates()
        {
            return _templates;
        }

        public TemplateDto GetTemplateById(Guid id)
        {
            return _templates.FirstOrDefault(t => t.Id == id);
        }
    }
}
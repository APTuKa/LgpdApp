using LgpdApp.Server.Data;
using LgpdApp.Server.DTOs;
using LgpdApp.Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace LgpdApp.Server.Services
{
    public class GamesService
    {
        private readonly ApplicationDbContext _context;

        public GamesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Game> CreateGameAsync(CreateGameRequest request, Guid createdBy)
        {
            // Проверка обязательных параметров
            if (!request.Parameters.ContainsKey("Режим игры"))
                throw new Exception("Параметр 'Режим игры' обязателен.");

            if (!request.Parameters.ContainsKey("Размер поля"))
                throw new Exception("Параметр 'Размер поля' обязателен.");

            var processedParams = new Dictionary<string, object>();

            foreach (var param in request.Parameters)
            {
                // Обработка фонов (поля и карточки)
                if (param.Key == "Фон поля" || param.Key == "Фон карточки")
                {
                    var value = param.Value;

                    if (Guid.TryParse(value, out var imageId))
                    {
                        var image = await _context.Images.FirstOrDefaultAsync(i => i.Id == imageId);
                        if (image != null)
                            processedParams[param.Key] = image.Path;
                        else
                            processedParams[param.Key] = "Изображение не найдено";
                    }
                    else if (value.StartsWith("#"))  // цветовая палитра
                    {
                        processedParams[param.Key] = value;
                    }
                    else
                    {
                        processedParams[param.Key] = "Неверный формат (ImageId или Hex)";
                    }
                }
                // Обработка списка карточек/пар
                else if (param.Key == "Карточки/Пары")
                {
                    try
                    {
                        var rawList = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(param.Value);
                        if (rawList == null || rawList.Count == 0)
                            throw new Exception("Список карточек/пар пустой или некорректный.");

                        // Проверка для режима Мемори: карточек должно быть парное число
                        if (request.Parameters["Режим игры"] == "Memori")
                        {
                            if (rawList.Count % 2 != 0)
                                throw new Exception("В режиме 'Мемори' количество карточек должно быть парным.");
                        }

                        var processedList = new List<Dictionary<string, string>>();

                        foreach (var item in rawList)
                        {
                            var processedItem = new Dictionary<string, string>();

                            foreach (var kv in item)
                            {
                                // Проверка для полей left/right/content: если это ImageId → заменяем на путь
                                if ((kv.Key == "left" || kv.Key == "right" || kv.Key == "content")
                                    && Guid.TryParse(kv.Value, out var imgId))
                                {
                                    var image = await _context.Images.FirstOrDefaultAsync(i => i.Id == imgId);
                                    if (image != null)
                                        processedItem[kv.Key] = image.Path;
                                    else
                                        processedItem[kv.Key] = "Изображение не найдено";
                                }
                                else
                                {
                                    processedItem[kv.Key] = kv.Value;  // обычный текст или флаг isPair/type
                                }
                            }

                            processedList.Add(processedItem);
                        }

                        processedParams[param.Key] = processedList;
                    }
                    catch
                    {
                        throw new Exception("Ошибка при обработке карточек/пар. Проверьте формат.");
                    }
                }
                // Обработка стандартных параметров (например, Режим игры, Размер поля)
                else
                {
                    processedParams[param.Key] = param.Value;
                }
            }

            var game = new Game
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                TemplateId = request.TemplateId,
                CreatedBy = createdBy,
                ParamsJson = JsonSerializer.Serialize(processedParams),
                CreatedAt = DateTime.UtcNow
            };

            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            return game;
        }

        public async Task<List<Game>> GetAllGamesAsync()
        {
            return await _context.Games.Include(g => g.Template).ToListAsync();
        }

        public async Task<Game> GetGameByIdAsync(Guid id)
        {
            return await _context.Games.Include(g => g.Template)
                                       .FirstOrDefaultAsync(g => g.Id == id);
        }
    }
}

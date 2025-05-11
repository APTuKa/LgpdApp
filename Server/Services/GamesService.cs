using LgpdApp.Server.Data;
using LgpdApp.Server.DTOs;
using LgpdApp.Server.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace LgpdApp.Server.Services
{
    public class GamesService : IGamesService
    {
        private readonly ApplicationDbContext _context;

        public GamesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Game> CreateGameAsync(CreateGameRequest request, Guid createdBy)
        {
            var processedParams = new Dictionary<string, object>();

            foreach (var param in request.Parameters)
            {
                if (param.Key == "Фон поля" || param.Key == "Фон карточки")
                {
                    var value = param.Value;
                    if (Guid.TryParse(value, out var imageId))
                    {
                        var image = await _context.Images.FirstOrDefaultAsync(i => i.Id == imageId);
                        if (image != null) processedParams[param.Key] = image.Path;
                    }
                    else if (value.StartsWith("#"))
                    {
                        processedParams[param.Key] = value;
                    }
                }
                else if (param.Key == "Карточки/Пары" || param.Key == "Варианты ответов" || param.Key == "Пузыри")
                {
                    try
                    {
                        var list = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(param.Value);
                        processedParams[param.Key] = list;
                    }
                    catch
                    {
                        throw new ArgumentException($"Ошибка при разборе списка '{param.Key}'.");
                    }
                }
                else if (param.Key == "Центральный объект")
                {
                    try
                    {
                        var obj = JsonConvert.DeserializeObject<Dictionary<string, object>>(param.Value);
                        processedParams[param.Key] = obj;
                    }
                    catch
                    {
                        throw new ArgumentException($"Ошибка при разборе объекта '{param.Key}'.");
                    }
                }
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
                ParamsJson = JsonConvert.SerializeObject(processedParams),
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
            return await _context.Games
                .Include(g => g.Template)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<bool> DeleteGameAsync(Guid id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null) return false;

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
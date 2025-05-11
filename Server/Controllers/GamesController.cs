using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using LgpdApp.Server.DTOs;
using LgpdApp.Server.Models;
using LgpdApp.Server.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using LgpdApp.Server.Data;

namespace LgpdApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Все методы защищены авторизацией
    public class GamesController : ControllerBase
    {
        private readonly GamesService _gamesService;
        private readonly ApplicationDbContext _context;

        public GamesController(GamesService gamesService, ApplicationDbContext context)
        {
            _gamesService = gamesService;
            _context = context;
        }

        /// <summary>
        /// Создание новой игры
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Logoped,Admin")] // Только логопед или админ могут создавать
        public async Task<IActionResult> CreateGame([FromBody] CreateGameRequest request)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { message = "Пользователь не авторизован." });

                var game = await _gamesService.CreateGameAsync(request, Guid.Parse(userId));
                return Ok(game);
            }
            catch (Exception ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Ошибка при создании игры",
                    Detail = ex.Message,
                    Instance = HttpContext.Request.Path
                });
            }
        }

        /// <summary>
        /// Получить все игры
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllGames()
        {
            try
            {
                var games = await _gamesService.GetAllGamesAsync();
                return Ok(games);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ProblemDetails
                {
                    Title = "Внутренняя ошибка сервера",
                    Detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Получить игру по ID
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetGameById(Guid id)
        {
            try
            {
                var game = await _gamesService.GetGameByIdAsync(id);
                if (game == null)
                    return NotFound(new ProblemDetails
                    {
                        Title = "Игра не найдена",
                        Detail = $"Игра с ID {id} отсутствует."
                    });

                return Ok(game);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ProblemDetails
                {
                    Title = "Внутренняя ошибка сервера",
                    Detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Удалить игру
        /// </summary>
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteGame(Guid id)
        {
            try
            {
                var success = await _gamesService.DeleteGameAsync(id);
                if (!success)
                    return NotFound(new ProblemDetails
                    {
                        Title = "Игра не найдена",
                        Detail = $"Игра с ID {id} отсутствует."
                    });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ProblemDetails
                {
                    Title = "Ошибка удаления игры",
                    Detail = ex.Message
                });
            }
        }
    }
}
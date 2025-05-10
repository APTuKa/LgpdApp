using LgpdApp.Server.DTOs;
using LgpdApp.Server.Models;
using LgpdApp.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LgpdApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GamesController : ControllerBase
    {
        private readonly GamesService _gamesService;

        public GamesController(GamesService gamesService)
        {
            _gamesService = gamesService;
        }

        [HttpPost]
       // [Authorize(Roles = "Admin,Logoped")]
        public async Task<IActionResult> CreateGame(CreateGameRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var game = await _gamesService.CreateGameAsync(request, userId);

                return Ok(new
                {
                    game.Id,
                    game.Name,
                    TemplateName = game.Template?.Name,
                    CreatedAt = game.CreatedAt
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetGames()
        {
            var games = await _gamesService.GetAllGamesAsync();
            return Ok(games.Select(g => new
            {
                g.Id,
                g.Name,
                TemplateName = g.Template?.Name,
                CreatedAt = g.CreatedAt
            }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGame(Guid id)
        {
            var game = await _gamesService.GetGameByIdAsync(id);
            if (game == null)
                return NotFound();

            return Ok(new
            {
                game.Id,
                game.Name,
                TemplateName = game.Template?.Name,
                Params = game.ParamsJson,
                CreatedAt = game.CreatedAt
            });
        }
    }
}

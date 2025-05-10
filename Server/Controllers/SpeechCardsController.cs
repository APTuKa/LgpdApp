using LgpdApp.Server.DTOs;
using LgpdApp.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LgpdApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SpeechCardsController : ControllerBase
    {
        private readonly SpeechCardsService _speechCardsService;

        public SpeechCardsController(SpeechCardsService speechCardsService)
        {
            _speechCardsService = speechCardsService;
        }

        // Логопед создает/обновляет речевую карту
        [HttpPost]
        [Authorize(Roles = "Logoped")]
        public async Task<IActionResult> CreateOrUpdateSpeechCard(SpeechCardDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var logopedId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

                // Проверяем, что ребенок привязан к этому логопеду
                var child = await _speechCardsService.GetChildAsync(dto.ChildId);
                if (child == null)
                    return NotFound(new { message = "Ребенок не найден." });

                if (child.LogopedId != logopedId)
                    return Forbid("Этот ребенок не привязан к вашему аккаунту.");

                var result = await _speechCardsService.CreateOrUpdateSpeechCardAsync(dto);
                return Ok(new
                {
                    result.Id,
                    result.ChildId,
                    result.Data
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{childId}")]
        [Authorize(Roles = "Logoped,Child")]
        public async Task<IActionResult> GetSpeechCard(Guid childId)
        {
            var currentUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var currentRole = User.FindFirst(ClaimTypes.Role)?.Value;

            var child = await _speechCardsService.GetChildAsync(childId);
            if (child == null)
                return NotFound(new { message = "Ребенок не найден." });

            if (currentRole == "Logoped")
            {
                // Проверка для логопеда: ребенок должен быть его
                if (child.LogopedId != currentUserId)
                    return Forbid("Этот ребенок не привязан к вашему аккаунту.");
            }
            else if (currentRole == "Child")
            {
                // Проверка для ребенка: может смотреть только свою карту
                if (childId != currentUserId)
                    return Forbid("Вы можете просматривать только свою речевую карту.");
            }

            var card = await _speechCardsService.GetSpeechCardAsync(childId);
            if (card == null)
                return NotFound(new { message = "Речевая карта не найдена." });

            return Ok(new
            {
                card.Id,
                card.ChildId,
                card.Data
            });
        }
    }
}

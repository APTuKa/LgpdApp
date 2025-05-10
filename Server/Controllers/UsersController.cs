using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LgpdApp.Server.DTOs;
using LgpdApp.Server.Models;
using LgpdApp.Server.Services;
using Microsoft.AspNetCore.Authorization;

using System.Security.Claims;

namespace LgpdApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : Controller
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var user = await _userService.GetUserByIdAsync(Guid.Parse(userId));
            if (user == null)
                return NotFound();

            return Ok(new
            {
                user.Id,
                user.FullName,
                user.Email,
                Role = user.Role.Name
            });
        }

        [HttpPost("create")]
        [Authorize(Roles = "Admin,Logoped")]
        public async Task<IActionResult> CreateUser(CreateUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Admin может создать любого; Логопед — только ребенка
                var currentRole = User.FindFirst(ClaimTypes.Role)?.Value;

                if (currentRole == "Logoped" && request.Role != "Child")
                    return Forbid("Логопед может создавать только детей");

                if (request.Role == "Child" && request.LogopedId == null)
                    request.LogopedId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

                var user = await _userService.CreateUserAsync(request);

                return Ok(new
                {
                    user.Id,
                    user.FullName,
                    user.Email,
                    Role = request.Role
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

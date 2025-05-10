using LgpdApp.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LgpdApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ImagesController : ControllerBase
    {
        private readonly ImagesService _imagesService;

        public ImagesController(ImagesService imagesService)
        {
            _imagesService = imagesService;
        }

        // Загрузка файла
        [HttpPost("upload")]
       // [Authorize(Roles = "Admin,Logoped")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "Файл обязателен." });

            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

                var image = await _imagesService.UploadImageAsync(file, userId);
                return Ok(new
                {
                    image.Id,
                    image.Path,
                    image.UploadedAt
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Список загруженных изображений
        [HttpGet]
        public async Task<IActionResult> GetImages()
        {
            var images = await _imagesService.GetAllImagesAsync();
            return Ok(images.Select(i => new
            {
                i.Id,
                i.Path,
                i.UploadedAt
            }));
        }
    }
}

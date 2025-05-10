using LgpdApp.Server.Data;
using LgpdApp.Server.DTOs;
using LgpdApp.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace LgpdApp.Server.Services
{
    public class SpeechCardsService
    {
        private readonly ApplicationDbContext _context;

        public SpeechCardsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SpeechCard> GetSpeechCardAsync(Guid childId)
        {
            return await _context.SpeechCards.FirstOrDefaultAsync(c => c.ChildId == childId);
        }

        public async Task<Child> GetChildAsync(Guid childId)
        {
            return await _context.Children.FirstOrDefaultAsync(c => c.Id == childId);
        }

        public async Task<SpeechCard> CreateOrUpdateSpeechCardAsync(SpeechCardDto dto)
        {
            var card = await _context.SpeechCards.FirstOrDefaultAsync(c => c.ChildId == dto.ChildId);

            if (card == null)
            {
                card = new SpeechCard
                {
                    Id = Guid.NewGuid(),
                    ChildId = dto.ChildId,
                    Data = dto.Data
                };
                _context.SpeechCards.Add(card);
            }
            else
            {
                card.Data = dto.Data;
                _context.SpeechCards.Update(card);
            }

            await _context.SaveChangesAsync();
            return card;
        }
    }
}

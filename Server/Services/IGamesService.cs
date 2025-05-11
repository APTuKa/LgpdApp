using LgpdApp.Server.DTOs;
using LgpdApp.Server.Models;
using System.Threading.Tasks;

namespace LgpdApp.Server.Services
{
    public interface IGamesService
    {
        Task<Game> CreateGameAsync(CreateGameRequest request, Guid createdBy);
        Task<List<Game>> GetAllGamesAsync();
        Task<Game> GetGameByIdAsync(Guid id);
        Task<bool> DeleteGameAsync(Guid id);
    }
}
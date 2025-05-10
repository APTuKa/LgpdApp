using LgpdApp.Server.Data;
using LgpdApp.Server.DTOs;
using LgpdApp.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace LgpdApp.Server.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> CreateUserAsync(CreateUserRequest request)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (existingUser != null)
                throw new Exception("Пользователь с таким email уже существует.");

            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == request.Role);
            if (role == null)
                throw new Exception("Указанная роль не найдена.");

            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = request.FullName,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                RoleId = role.Id
            };

            _context.Users.Add(user);

            // Если создается ребенок — добавим запись в таблицу Children
            if (role.Name == "Child")
            {
                if (request.LogopedId == null)
                    throw new Exception("Для создания ребенка необходимо указать логопеда.");

                _context.Children.Add(new Child
                {
                    Id = user.Id, // Child.Id = User.Id
                    LogopedId = request.LogopedId
                });
            }

            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}

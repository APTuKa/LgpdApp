using Microsoft.EntityFrameworkCore;
using LgpdApp.Server.Models;

namespace LgpdApp.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Child> Children { get; set; }
        public DbSet<SpeechCard> SpeechCards { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Image> Images { get; set; }
    }
}

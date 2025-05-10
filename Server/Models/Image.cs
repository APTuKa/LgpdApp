namespace LgpdApp.Server.Models
{
    public class Image
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string Path { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; }
    }
}

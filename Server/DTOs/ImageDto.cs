namespace LgpdApp.Server.DTOs
{
    public class ImageDto
    {
        public Guid Id { get; set; }
        public string Path { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}

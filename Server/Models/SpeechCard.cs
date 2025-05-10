namespace LgpdApp.Server.Models
{
    public class SpeechCard
    {
        public Guid Id { get; set; }
        public Guid ChildId { get; set; }
        public string Data { get; set; }  // Можно сделать структуру JSON или просто текст для начала

        public Child Child { get; set; }  // Навигационное свойство
    }
}

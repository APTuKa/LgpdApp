namespace LgpdApp.Server.Models
{
    public class Card
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public string ImagePath { get; set; }
        public bool IsPair { get; set; } // Указывает, парная ли карточка (можно использовать при развитии логики)

        public Game Game { get; set; }
    }
}
